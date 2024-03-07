using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zoomies;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [Header("Status Bars")]
        [SerializeField] private StatBar _hungerBar;
        [SerializeField] private StatBar _thirstBar;
        [SerializeField] private StatBar _boredomBar;

        [Header("Status Texts")]
        [SerializeField] private TMP_Text _hungerStatusText;
        [SerializeField] private TMP_Text _thirstStatusText;
        [SerializeField] private TMP_Text _boredomStatusText;

        [Header("Action Buttons")]
        [SerializeField] private Button _eatButton;
        [SerializeField] private Button _poopButton;
        [SerializeField] private Button _drinkButton;
        [SerializeField] private Button _peeButton;
        [SerializeField] private Button _zoomButton;
        [SerializeField] private Button _sleepButton;
        
        [Header("Minigames")]
        [SerializeField] private ZoomiesMinigame _zoomiesMinigame;
        
        private GameplayState _gameplayState;
        
        private enum GameplayState
        {
            MainPanel,
            Minigame,
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        private void OnEnable()
        {
            //STAT EVENTS.
            _hungerBar.Emptied += OnHungerEmptied;
            _hungerBar.OK += OnHungerOK;
            _hungerBar.Filled += OnHungerFilled;
            _thirstBar.Emptied += OnThirstEmptied;
            _thirstBar.OK += OnThirstOK;
            _thirstBar.Filled += OnThirstFilled;
            _boredomBar.Emptied += OnBoredomEmptied;
            _boredomBar.OK += OnBoredomOK;
            _boredomBar.Filled += OnBoredomFilled;
                
            //BUTTON EVENTS.
            _eatButton.onClick.AddListener(OnEatButtonClicked);
            _poopButton.onClick.AddListener(OnPoopButtonClicked);
            _drinkButton.onClick.AddListener(OnDrinkButtonClicked);
            _peeButton.onClick.AddListener(OnPeeButtonClicked);
            _zoomButton.onClick.AddListener(OnZoomButtonClicked);
            _sleepButton.onClick.AddListener(OnSleepButtonClicked);
        }
        
        private void OnDisable()
        {
            //STAT EVENTS.
            _hungerBar.Emptied -= OnHungerEmptied;
            _hungerBar.OK -= OnHungerOK;
            _hungerBar.Filled -= OnHungerFilled;
            _thirstBar.Emptied -= OnThirstEmptied;
            _thirstBar.OK -= OnThirstOK;
            _thirstBar.Filled -= OnThirstFilled;
            _boredomBar.Emptied -= OnBoredomEmptied;
            _boredomBar.OK -= OnBoredomOK;
            _boredomBar.Filled -= OnBoredomFilled;
            
            //BUTTON EVENTS.
            _eatButton.onClick.RemoveListener(OnEatButtonClicked);
            _poopButton.onClick.RemoveListener(OnPoopButtonClicked);
            _drinkButton.onClick.RemoveListener(OnDrinkButtonClicked);
            _peeButton.onClick.RemoveListener(OnPeeButtonClicked);
            _zoomButton.onClick.RemoveListener(OnZoomButtonClicked);
            _sleepButton.onClick.RemoveListener(OnSleepButtonClicked);
        }

        //--------------------------------------------------------------------------------------------------------------
        
        #region Stat Events
       
        private void OnHungerEmptied()
        {
            Debug.Log("Hunger Emptied.");
            
            _hungerStatusText.text = "Starving!";
        }
        
        private void OnHungerOK()
        {
            Debug.Log("Hunger OK.");
            
            _hungerStatusText.text = "OK.";
        }

        private void OnHungerFilled()
        {
            Debug.Log("Hunger Filled.");
            
            _hungerStatusText.text = "Full!";
        }

        private void OnThirstEmptied()
        {
            Debug.Log("Thirst Emptied.");
            
            _thirstStatusText.text = "Dehydrated!";
        }
        
        private void OnThirstOK()
        {
            Debug.Log("Thirst OK.");
            
            _thirstStatusText.text = "OK.";
        }

        private void OnThirstFilled()
        {
            Debug.Log("Thirst Filled.");
            
            _thirstStatusText.text = "Hydrated!";
        }

        private void OnBoredomEmptied()
        {
            Debug.Log("Boredom Emptied.");
            
            _boredomStatusText.text = "Bored!";
        }
        
        private void OnBoredomOK()
        {
            Debug.Log("Boredom OK.");
            
            _boredomStatusText.text = "OK.";
        }

        private void OnBoredomFilled()
        {
            Debug.Log("Boredom Filled.");
            
            _boredomStatusText.text = "Entertained!";
        }
        
        #endregion
        
        //--------------------------------------------------------------------------------------------------------------
        
        #region Button Events
        
        private void OnEatButtonClicked()
        {
            if (_gameplayState == GameplayState.Minigame) return;
            
            _hungerBar.ChangeValueBy(1);
        }
        
        private void OnPoopButtonClicked()
        {
            if (_gameplayState == GameplayState.Minigame) return;
            
            _hungerBar.ChangeValueBy(-1);
        }

        private void OnDrinkButtonClicked()
        {
            if (_gameplayState == GameplayState.Minigame) return;
            
            _thirstBar.ChangeValueBy(1);
        }

        private void OnPeeButtonClicked()
        {
            if (_gameplayState == GameplayState.Minigame) return;
            
            _thirstBar.ChangeValueBy(-1);
        }

        private void OnZoomButtonClicked()
        {
            if (_gameplayState == GameplayState.Minigame) return;
            
            _gameplayState = GameplayState.Minigame;
            
            DisableActionButtons();

            _zoomiesMinigame.Finished += OnZoomiesMinigameFinished;
            _zoomiesMinigame.gameObject.SetActive(true);
            
            Debug.Log("Zooming!");
        }

        private void OnSleepButtonClicked()
        {
            if (_gameplayState == GameplayState.Minigame) return;
            
            _boredomBar.ChangeValueBy(-1);
        }
        
        #endregion

        //--------------------------------------------------------------------------------------------------------------
        
        private void OnZoomiesMinigameFinished()
        {
            _zoomiesMinigame.Finished -= OnZoomiesMinigameFinished;
            _zoomiesMinigame.gameObject.SetActive(false);

            Debug.Log("Zooming Finished!");

            _gameplayState = GameplayState.MainPanel;
            
            EnableActionButtons();
            
            _hungerBar.ChangeValueBy(-1);
            _boredomBar.ChangeValueBy(2);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        private void Start()
        {
            _gameplayState = GameplayState.MainPanel;
            
            _hungerBar.SetValue(3, false);
            _thirstBar.SetValue(3, false);
            _boredomBar.SetValue(3, false);
        }
        
        //--------------------------------------------------------------------------------------------------------------
        
        private void DisableActionButtons()
        {
            _eatButton.interactable = false;
            _poopButton.interactable = false;
            _drinkButton.interactable = false;
            _peeButton.interactable = false;
            _zoomButton.interactable = false;
            _sleepButton.interactable = false;
        }
        
        private void EnableActionButtons()
        {
            _eatButton.interactable = true;
            _poopButton.interactable = true;
            _drinkButton.interactable = true;
            _peeButton.interactable = true;
            _zoomButton.interactable = true;
            _sleepButton.interactable = true;
        }
        
        //--------------------------------------------------------------------------------------------------------------
    }
}