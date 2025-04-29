using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using System;
using Unity.Services.Core;

namespace Game.Relay
{
    public class RelayManager : MonoBehaviour
    {
        public static RelayManager Instance;

        // No longer Storing JoinCode here, it's fetch from lobby Data  
        // public string JoinCode { get; private set; }

        private UnityTransport _transport; //Cache the transport


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private UnityTransport GetTransport()
        {
            if (_transport == null)
            {
                //Ensure NetworkManager and its transport are ready
                if (NetworkManager.Singleton == null)
                {
                    Debug.LogError("NetworkManager Singleton not found!");
                    return null;
                }

                // getting UnityTransport Component from NetworkManager
                _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

                if (_transport == null)
                {
                    Debug.LogError("UnityTransport component not found on NetworkManager");
                    return null;
                }
            }
            return _transport;
        }



        public async Task<string> StartHostWithRelayAsync()
        {
            await UnityServices.InitializeAsync();

            UnityTransport transport = GetTransport();
            if (transport == null) return null;

            try
            {
                // Request allocation (Max connections = Max Players, including host)
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);

                //Configure transport
                transport.SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));

                //Get join code
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                return NetworkManager.Singleton.StartHost() ? joinCode : null;
            }
            catch (RelayServiceException e)
            {
                Debug.LogError($"Failed to create Relay allocation: {e}");
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"An unexpected error occurred during Relay creation: {e}");
                return null;
            }
        }



        public async Task<bool> StartClientWithRelayAsync(string joinCode)
        {

            UnityTransport transport = GetTransport();
            if (transport == null) return false;

            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError("JoinRelayAsync called with null or empty joincode.");
                return false;
            }

            Debug.Log($"Attempting to start client with code: {joinCode}");
            try
            {
                // Request to join allocation
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                // Configure transport
                transport.SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "dtls"));

                return NetworkManager.Singleton.StartClient();

            }
            catch (RelayServiceException e)
            {
                // Specific check for 404 Not Found
                if (e.Message.Contains("404") || e.Message.ToLower().Contains("not found"))
                {
                    Debug.LogError($"Failed to join Relay: Join Code '{joinCode}' not found or expired. {e}");
                }
                else
                {
                    Debug.LogError($"Failed to join Relay allocation with code '{joinCode}': {e}");
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError($"An unexpected error occurred during Relay joining: {e}");
                return false;
            }
        }
    }
}
