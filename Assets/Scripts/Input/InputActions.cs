//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/Scripts/Input/InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Input
{
    public partial class @InputActions : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""In Game"",
            ""id"": ""425d4ee2-9770-4d46-a142-e835fff63a44"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""d4e39949-6040-41b2-9d5e-1346a25721d7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""6645ce86-5f04-49dc-8862-8c04af563f7e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Potion Swap"",
                    ""type"": ""Button"",
                    ""id"": ""4310bb5c-a5ca-4ee2-8c42-54f25953ceb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""0a3e023b-c4a2-4a35-82ce-1b7bdd0515d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Primary Attack"",
                    ""type"": ""Button"",
                    ""id"": ""9f2090c2-cc27-49b5-af1c-c8554e00b59d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Secondary Attack"",
                    ""type"": ""Button"",
                    ""id"": ""bfe333ed-981e-48f3-a8ab-9a0feac84410"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD/Arrows"",
                    ""id"": ""056aa27c-a2b2-4436-a063-aa0d9ad79588"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""da094e1a-8a7c-4d61-9c5b-a899db4410ff"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""969097f7-da3d-4080-a0fb-f02cd19cf46b"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ca599159-2f64-4c70-95bd-3e7ed24b89b3"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4053f43c-d64f-41f9-8f20-453d49662cc3"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e17b328f-7c96-4dfe-a5a4-18f45ee23aea"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9500d4de-3c4a-4c6d-9aa2-174e021dd9dd"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""78eba356-ad2e-430d-9e71-f81709940aa0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5ddb184c-a5ba-4466-80b2-26285de7601d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""794d1b13-3574-4bc2-b134-b036c116476c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee888edb-c786-48f6-81e4-d54426512cc1"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db3560d3-bb8d-4a42-af4c-209edad4c0fd"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""71429b2f-33e7-4e19-8f7a-0882d5512602"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Potion Swap"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""e8a9c7e3-2710-490e-a6cb-f7f3d9289dad"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Potion Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""068e0939-4792-412d-9526-bf82edf2bf12"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Potion Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""a1972f75-28af-4b06-ac10-aaa65543a100"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Potion Swap"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ba66895a-40b7-41d5-9ab4-fe86859410f2"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Potion Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""70ad1af0-0500-4c8b-aa94-3dca2bebf799"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Potion Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""be789045-9f53-42fb-8070-99a3c361b652"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""010cef9d-dc88-4194-b390-5a0ecd2403cb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23154cb6-b2e0-4e0d-aacc-25e0d58c6517"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Primary Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""642fc986-b9de-47a9-b176-84c47f8d3452"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Primary Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bf1bb64-00a8-4790-b510-c99e68eb5c09"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Secondary Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6513d2b-6e47-4319-9d97-310560f20e85"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Secondary Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""3e93cfee-e97f-4308-aff3-dfc5b2d088e8"",
            ""actions"": [
                {
                    ""name"": ""SelectorMovement"",
                    ""type"": ""Value"",
                    ""id"": ""59b9eb1a-47d8-40ca-9d96-e5dd14bf9149"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PointerPos"",
                    ""type"": ""Value"",
                    ""id"": ""46ce0fa9-4926-4ef3-b2db-edce18d80dbe"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""abdadd34-feae-46b4-8a81-4ac2dd0c45a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""ea5d3230-6b13-4596-91a4-b5944b2f889b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD/Arrows"",
                    ""id"": ""7dc0a959-3714-4793-bbf1-b019da4cfed1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b06b7be0-c606-47f6-b8f4-2609ba83ae26"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0b3f7b52-f967-4de6-8729-3eb039d55d0c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5a9dd9b5-5307-4dee-b65b-805495da62fc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9aec708c-adc2-44ab-b08a-0f3fa5f01291"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cb19876b-26ec-4644-a361-5a6d3eaf9975"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""be453354-4a0e-49f2-9feb-29fef4884074"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""92ab1d2d-2021-44d2-8edd-78b4a6efeff6"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6aa0a84c-29ea-4078-ad30-3cf25731df8a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""96616ecc-44b0-46a2-819c-847033c7741d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectorMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4a2d070-a1e7-4818-9f17-6bc6517250d7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""PointerPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b883b1dd-16b1-4113-8461-7308491ee105"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6cd6240-498a-41c4-a114-45e91c57e2b7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a0f65b6-6361-4388-a722-f9c2d64c4185"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48d51b6b-1926-4825-97a8-97e878e40124"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c89bbf8c-ce87-40bd-9ce6-44509356abee"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f150d25c-7e69-44f4-81c8-8e717ae01d50"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0ecd6ff-929e-4e38-b3a8-048fe3efc03a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // In Game
            m_InGame = asset.FindActionMap("In Game", throwIfNotFound: true);
            m_InGame_Movement = m_InGame.FindAction("Movement", throwIfNotFound: true);
            m_InGame_Aim = m_InGame.FindAction("Aim", throwIfNotFound: true);
            m_InGame_PotionSwap = m_InGame.FindAction("Potion Swap", throwIfNotFound: true);
            m_InGame_Interact = m_InGame.FindAction("Interact", throwIfNotFound: true);
            m_InGame_PrimaryAttack = m_InGame.FindAction("Primary Attack", throwIfNotFound: true);
            m_InGame_SecondaryAttack = m_InGame.FindAction("Secondary Attack", throwIfNotFound: true);
            // Menu
            m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
            m_Menu_SelectorMovement = m_Menu.FindAction("SelectorMovement", throwIfNotFound: true);
            m_Menu_PointerPos = m_Menu.FindAction("PointerPos", throwIfNotFound: true);
            m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
            m_Menu_Back = m_Menu.FindAction("Back", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }
        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }
        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // In Game
        private readonly InputActionMap m_InGame;
        private IInGameActions m_InGameActionsCallbackInterface;
        private readonly InputAction m_InGame_Movement;
        private readonly InputAction m_InGame_Aim;
        private readonly InputAction m_InGame_PotionSwap;
        private readonly InputAction m_InGame_Interact;
        private readonly InputAction m_InGame_PrimaryAttack;
        private readonly InputAction m_InGame_SecondaryAttack;
        public struct InGameActions
        {
            private @InputActions m_Wrapper;
            public InGameActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_InGame_Movement;
            public InputAction @Aim => m_Wrapper.m_InGame_Aim;
            public InputAction @PotionSwap => m_Wrapper.m_InGame_PotionSwap;
            public InputAction @Interact => m_Wrapper.m_InGame_Interact;
            public InputAction @PrimaryAttack => m_Wrapper.m_InGame_PrimaryAttack;
            public InputAction @SecondaryAttack => m_Wrapper.m_InGame_SecondaryAttack;
            public InputActionMap Get() { return m_Wrapper.m_InGame; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
            public void SetCallbacks(IInGameActions instance)
            {
                if (m_Wrapper.m_InGameActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMovement;
                    @Aim.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnAim;
                    @PotionSwap.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPotionSwap;
                    @PotionSwap.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPotionSwap;
                    @PotionSwap.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPotionSwap;
                    @Interact.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnInteract;
                    @PrimaryAttack.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnPrimaryAttack;
                    @PrimaryAttack.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnPrimaryAttack;
                    @PrimaryAttack.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnPrimaryAttack;
                    @SecondaryAttack.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnSecondaryAttack;
                    @SecondaryAttack.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnSecondaryAttack;
                    @SecondaryAttack.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnSecondaryAttack;
                }
                m_Wrapper.m_InGameActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                    @PotionSwap.started += instance.OnPotionSwap;
                    @PotionSwap.performed += instance.OnPotionSwap;
                    @PotionSwap.canceled += instance.OnPotionSwap;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @PrimaryAttack.started += instance.OnPrimaryAttack;
                    @PrimaryAttack.performed += instance.OnPrimaryAttack;
                    @PrimaryAttack.canceled += instance.OnPrimaryAttack;
                    @SecondaryAttack.started += instance.OnSecondaryAttack;
                    @SecondaryAttack.performed += instance.OnSecondaryAttack;
                    @SecondaryAttack.canceled += instance.OnSecondaryAttack;
                }
            }
        }
        public InGameActions @InGame => new InGameActions(this);

        // Menu
        private readonly InputActionMap m_Menu;
        private IMenuActions m_MenuActionsCallbackInterface;
        private readonly InputAction m_Menu_SelectorMovement;
        private readonly InputAction m_Menu_PointerPos;
        private readonly InputAction m_Menu_Select;
        private readonly InputAction m_Menu_Back;
        public struct MenuActions
        {
            private @InputActions m_Wrapper;
            public MenuActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @SelectorMovement => m_Wrapper.m_Menu_SelectorMovement;
            public InputAction @PointerPos => m_Wrapper.m_Menu_PointerPos;
            public InputAction @Select => m_Wrapper.m_Menu_Select;
            public InputAction @Back => m_Wrapper.m_Menu_Back;
            public InputActionMap Get() { return m_Wrapper.m_Menu; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
            public void SetCallbacks(IMenuActions instance)
            {
                if (m_Wrapper.m_MenuActionsCallbackInterface != null)
                {
                    @SelectorMovement.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelectorMovement;
                    @SelectorMovement.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelectorMovement;
                    @SelectorMovement.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelectorMovement;
                    @PointerPos.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnPointerPos;
                    @PointerPos.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnPointerPos;
                    @PointerPos.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnPointerPos;
                    @Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                    @Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                    @Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                    @Back.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                    @Back.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                    @Back.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnBack;
                }
                m_Wrapper.m_MenuActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @SelectorMovement.started += instance.OnSelectorMovement;
                    @SelectorMovement.performed += instance.OnSelectorMovement;
                    @SelectorMovement.canceled += instance.OnSelectorMovement;
                    @PointerPos.started += instance.OnPointerPos;
                    @PointerPos.performed += instance.OnPointerPos;
                    @PointerPos.canceled += instance.OnPointerPos;
                    @Select.started += instance.OnSelect;
                    @Select.performed += instance.OnSelect;
                    @Select.canceled += instance.OnSelect;
                    @Back.started += instance.OnBack;
                    @Back.performed += instance.OnBack;
                    @Back.canceled += instance.OnBack;
                }
            }
        }
        public MenuActions @Menu => new MenuActions(this);
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        public interface IInGameActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
            void OnPotionSwap(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnPrimaryAttack(InputAction.CallbackContext context);
            void OnSecondaryAttack(InputAction.CallbackContext context);
        }
        public interface IMenuActions
        {
            void OnSelectorMovement(InputAction.CallbackContext context);
            void OnPointerPos(InputAction.CallbackContext context);
            void OnSelect(InputAction.CallbackContext context);
            void OnBack(InputAction.CallbackContext context);
        }
    }
}