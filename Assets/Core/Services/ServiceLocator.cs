using System;
using System.Threading;
using Core.Services.PlayerData;
using EasyButtons;
using UnityEngine;

namespace Core.Services
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance {get; private set;}
        
        public PlayerDataService PlayerDataService {get; private set;}
        
        public void Awake()
        {
            if (Instance!=null)
            {
                Destroy(gameObject);
                return;
            }
            
            PlayerDataService = new PlayerDataService();
            
            Application.targetFrameRate = 60;
            Instance = this;
        }
    }
}
