/*
 * Copyright (c) 2023 Black Whale Studio. All rights reserved.
 *
 * This software is the intellectual property of Black Whale Studio. Direct use, copying, or distribution of this code in its original or only slightly modified form is strictly prohibited. Significant modifications or derivations are required for any use.
 *
 * If this code is intended to be used in a commercial setting, you must contact Black Whale Studio for explicit permission.
 *
 * For the full licensing terms and conditions, visit:
 * https://blackwhale.dev/
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT ANY WARRANTIES OR CONDITIONS.
 *
 * For questions or to join our community, please visit our Discord: https://discord.gg/55gtTryfWw
 */

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

namespace Keyboard
{
    public class KeyboardManager : MonoBehaviour
    {
        [Header("Keyboard Setup")]
        [SerializeField] private KeyChannel keyChannel;
        [SerializeField] private Button deleteButton;
        [SerializeField] private Button switchButton;
        [SerializeField] private string switchToNumbers = "Numbers";
        [SerializeField] private string switchToLetter = "Letters";
        [SerializeField] private GameObject validationMessageBackground;
        [SerializeField] private TMP_Text validationMessage;
        [SerializeField] private GameObject FAButton;

        private TextMeshProUGUI switchButtonText;

        [Header("Keyboards")]
        [SerializeField] private GameObject lettersKeyboard;
        [SerializeField] private GameObject numbersKeyboard;
        [SerializeField] private GameObject specialCharactersKeyboard;

        [Header("Shift/Caps Lock Button")]
        [SerializeField] private Button shiftButton;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite activeSprite;

        [Header("Switch Number/Special Button")]
        [SerializeField] private Button switchNumberSpecialButton;
        [SerializeField] private string numbersString = "Numbers";
        [SerializeField] private string specialString = "Special";

        private TextMeshProUGUI switchNumSpecButtonText;

        [Header("Keyboard Button Colors")]
        [SerializeField] private Color normalColor = Color.black;
        [SerializeField] private Color highlightedColor = Color.yellow;
        [SerializeField] private Color pressedColor = Color.red;
        [SerializeField] private Color selectedColor = Color.blue;

        [Header("Output Field Settings")]
        [SerializeField] public TMP_InputField outputField;
        [SerializeField] private Button enterButton;
        [SerializeField] private int maxCharacters = 15;

        [Header("Particle Systems manager")]
        [SerializeField] private ParticleSystem RayTwo;
        [SerializeField] private ParticleSystem RayThree;
        [SerializeField] private ParticleSystem RayFour;
        [SerializeField] private ParticleSystem RayFive;

        private ColorBlock shiftButtonColors;
        private bool isFirstKeyPress = true;
        private bool keyHasBeenPressed;
        private bool shiftActive;
        private bool capsLockActive;
        private float lastShiftClickTime;
        private float shiftDoubleClickDelay = 0.5f;
        private int passwordLevel = -1;

        public UnityEvent onKeyboardModeChanged;

        private void Awake()
        {
            shiftButtonColors = shiftButton.colors;

            CheckTextLength();

            numbersKeyboard.SetActive(false);
            specialCharactersKeyboard.SetActive(false);
            lettersKeyboard.SetActive(true);

            deleteButton.onClick.AddListener(OnDeletePress);
            enterButton.onClick.AddListener(OnEnterPress);
            switchButton.onClick.AddListener(OnSwitchPress);
            shiftButton.onClick.AddListener(OnShiftPress);
            switchNumberSpecialButton.onClick.AddListener(SwitchBetweenNumbersAndSpecialCharacters);
            switchButtonText = switchButton.GetComponentInChildren<TextMeshProUGUI>();
            switchNumSpecButtonText = switchNumberSpecialButton.GetComponentInChildren<TextMeshProUGUI>();
            keyChannel.RaiseKeyColorsChangedEvent(normalColor, highlightedColor, pressedColor, selectedColor);

            switchNumberSpecialButton.gameObject.SetActive(false);
            numbersKeyboard.SetActive(false);
            specialCharactersKeyboard.SetActive(false);
        }

        private void OnDestroy()
        {
            deleteButton.onClick.RemoveListener(OnDeletePress);
            enterButton.onClick.RemoveListener(OnEnterPress);
            switchButton.onClick.RemoveListener(OnSwitchPress);
            shiftButton.onClick.RemoveListener(OnShiftPress);
            switchNumberSpecialButton.onClick.RemoveListener(SwitchBetweenNumbersAndSpecialCharacters);
        }

        private void OnEnable() => keyChannel.OnKeyPressed += KeyPress;

        private void OnDisable() => keyChannel.OnKeyPressed -= KeyPress;

        private void KeyPress(string key)
        {
            keyHasBeenPressed = true;
            bool wasShiftActive = shiftActive;
            DeactivateShift();

            string textToInsert;
            if (wasShiftActive || capsLockActive)
            {
                textToInsert = key.ToUpper();
            }
            else
            {
                textToInsert = key.ToLower();
            }

            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            outputField.text = outputField.text.Remove(startPos, endPos - startPos);
            outputField.text = outputField.text.Insert(Mathf.Max(startPos, outputField.text.Length), textToInsert);

            outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos + textToInsert.Length;

            if (isFirstKeyPress)
            {
                isFirstKeyPress = false;
                keyChannel.onFirstKeyPress.Invoke();
            }

            CheckTextLength();
        }

        private void OnDeletePress()
        {
            if (string.IsNullOrEmpty(outputField.text)) return;
            int startPos = Mathf.Min(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);
            startPos = Mathf.Max(startPos, outputField.text.Length);
            int endPos = Mathf.Max(outputField.selectionAnchorPosition, outputField.selectionFocusPosition);

            if (endPos > startPos)
            {
                outputField.text = outputField.text.Remove(startPos, endPos - startPos);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos;
            }
            else if (startPos > 0)
            {
                outputField.text = outputField.text.Remove(startPos - 1, 1);
                outputField.selectionAnchorPosition = outputField.selectionFocusPosition = startPos - 1;
            }

            CheckTextLength();
        }

        private void CheckTextLength()
        {
            int currentLength = outputField.text.Length;

            // Raise event to enable or disable keys based on the text length
            bool keysEnabled = currentLength < maxCharacters;
            keyChannel.RaiseKeysStateChangeEvent(keysEnabled);

            enterButton.interactable = true;

            // Always enable the delete button, regardless of the text length
            deleteButton.interactable = true;

            // Disable shift/caps lock if maximum text length is reached
            if (currentLength != maxCharacters) return;
            DeactivateShift();
            capsLockActive = false;
            UpdateShiftButtonAppearance();
        }

        private void OnSwitchPress()
        {
            if (passwordLevel == 0 || passwordLevel == 1) return;
            if (lettersKeyboard.activeSelf)
            {
                lettersKeyboard.SetActive(false);
                numbersKeyboard.SetActive(true);
                specialCharactersKeyboard.SetActive(false);
                switchNumberSpecialButton.gameObject.SetActive(true);

                // Set buttons' text
                switchButtonText.text = switchToNumbers;
                switchNumSpecButtonText.text = specialString;
            }
            else
            {
                lettersKeyboard.SetActive(true);
                numbersKeyboard.SetActive(false);
                specialCharactersKeyboard.SetActive(false);
                switchNumberSpecialButton.gameObject.SetActive(false);

                // Set buttons' text
                switchButtonText.text = switchToLetter;
                switchNumSpecButtonText.text = specialString;
            }
            DeactivateShift();
            onKeyboardModeChanged?.Invoke();
        }


        private void OnShiftPress()
        {
            if (passwordLevel == 0) return;
            if (capsLockActive)
            {
                // If Caps Lock is active, deactivate it
                capsLockActive = false;
                shiftActive = false;
            }
            else switch (shiftActive)
                {
                    case true when !keyHasBeenPressed && Time.time - lastShiftClickTime < shiftDoubleClickDelay:
                        // If Shift is active, a key has not been pressed, and Shift button was double clicked, activate Caps Lock
                        capsLockActive = true;
                        shiftActive = false;
                        break;
                    case true when !keyHasBeenPressed:
                        // If Shift is active, a key has not been pressed, deactivate Shift
                        shiftActive = false;
                        break;
                    case false:
                        // If Shift is not active and Shift button was clicked once, activate Shift
                        shiftActive = true;
                        break;
                }

            lastShiftClickTime = Time.time;
            UpdateShiftButtonAppearance();
            onKeyboardModeChanged?.Invoke();
        }

        public void OnEnterPress()
        {
            // depending on the level (how far in the game player is) show labels on right/wrong password
            switch (passwordLevel)
            {
                case 0:
                    validationMessageBackground.SetActive(true);
                    //if output by the user is correct
                    if (outputField.text.Length >= 8 && outputField.text.Length <= 10)
                    {
                        validationMessage.SetText("All passwords with less than 14 characters are hacked in less than <color=red><i>2 hours.</i></color>\nLet's work on that and make attacker's life harder.\nGo to the next lit mat.");
                        validationMessage.color = new Color(0, 255, 0, 255);
                        RayTwo.Play();
                    }
                    //if output by the user is wrong
                    else
                    {
                        validationMessage.SetText("Attackers are after your password!\nBetter enter between 8 and 10 characters.");
                        validationMessage.color = new Color(255, 0, 0, 255);
                    }
                    break;
                case 1:
                    validationMessageBackground.SetActive(true);
                    if (outputField.text.Length >= 8 && outputField.text.Any(char.IsUpper))
                    {
                        validationMessage.SetText("Passwords under 14 characters with only upper and lowercase letter are hacked in less than <color=red><i>a day.</i></color>\nLet's work on that and make attacker's life harder.\nGo to the next lit mat.");
                        validationMessage.color = new Color(0, 255, 0, 255);
                        RayThree.Play();
                    }
                    else
                    {
                        validationMessage.SetText("Attackers are after your password!\nBetter enter an upper case character.");
                        validationMessage.color = new Color(255, 0, 0, 255);
                    }
                    break;
                case 2:
                    validationMessageBackground.SetActive(true);
                    if (outputField.text.Length >= 8 && outputField.text.Any(char.IsUpper) && outputField.text.Any(char.IsDigit))
                    {
                        validationMessage.SetText("Passwords under 14 characters without a special character are hacked in less than <color=red><i>a week.</i></color>\nLet's work on that and make attacker's life harder.\nGo to the next lit mat.");
                        validationMessage.color = new Color(0, 255, 0, 255);
                        RayFour.Play();
                    }
                    else
                    {
                        validationMessage.SetText("Attackers are after your password!\nBetter enter a digit.");
                        validationMessage.color = new Color(255, 0, 0, 255);
                    }
                    break;
                case 3:
                    validationMessageBackground.SetActive(true);
                    if (outputField.text.Any(char.IsUpper) && outputField.text.Any(char.IsDigit) && outputField.text.Any(char.IsSymbol))
                    {
                        validationMessage.SetText("Great job! Attackers will need 300 years to hack this password.\nThat's <i>almost</i> uneatable!.\nGo to the next lit mat to see some further tips.");
                        validationMessage.color = new Color(0, 255, 0, 255);
                        RayFive.Play();
                    }
                    else
                    {
                        validationMessage.SetText("Attackers are after your password!\nBetter enter a special character.");
                        validationMessage.color = new Color(255, 0, 0, 255);
                    }
                    break;
                default:
                    break;
            }
        }

        public void IncreaseLevel()
        {
            passwordLevel += 1;
        }

        public void ResetValidationMessage()
        {
            validationMessageBackground.SetActive(false);
        }

        public void Show2FAButton()
        {
            if (passwordLevel == 4)
                FAButton.SetActive(true);
        }

        public void DeactivateShift()
        {
            if (shiftActive && !capsLockActive && keyHasBeenPressed)
            {
                shiftActive = false;
                UpdateShiftButtonAppearance();
                onKeyboardModeChanged?.Invoke();
            }

            keyHasBeenPressed = false;
        }

        public bool IsShiftActive() => shiftActive;

        public bool IsCapsLockActive() => capsLockActive;

        public void ClearInputField(){
            outputField.text = "";
        }

        private void SwitchBetweenNumbersAndSpecialCharacters()
        {
            if (lettersKeyboard.activeSelf) return;

            // Switch between numbers and special characters keyboard
            bool isNumbersKeyboardActive = numbersKeyboard.activeSelf;
            numbersKeyboard.SetActive(!isNumbersKeyboardActive);
            specialCharactersKeyboard.SetActive(isNumbersKeyboardActive);

            switchNumSpecButtonText.text = switchNumSpecButtonText.text == specialString ? numbersString : specialString;

            onKeyboardModeChanged?.Invoke();
        }

        private void UpdateShiftButtonAppearance()
        {
            if (capsLockActive)
            {
                shiftButtonColors.normalColor = highlightedColor;
                buttonImage.sprite = activeSprite;
            }
            else if (shiftActive)
            {
                shiftButtonColors.normalColor = highlightedColor;
                buttonImage.sprite = defaultSprite;
            }
            else
            {
                shiftButtonColors.normalColor = normalColor;
                buttonImage.sprite = defaultSprite;
            }

            shiftButton.colors = shiftButtonColors;
        }
    }
}