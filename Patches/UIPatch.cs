using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace UltraTelephone.Patches
{
    [HarmonyPatch(typeof(OptionsMenuToManager), "Start")]
    public static class UIPatch // IF WE USED UMM THIS ENTIRE FUCKING SECTION WOULDN'T BE NECCESARY AS I (TEMPERZ87) ALREADY IMPLEMENTED THIS FUNCTIONALITY
    {
        public static void Postfix(OptionsMenuToManager __instance)
        {
            //Application.OpenURL("https://github.com/Temperz87/ultra-mod-manager/blob/7ef4a1626a6f6a97c53648cd970c26d204d8f174/UK%20Mod%20Manager/Harmony%20Patches/Mod%20UI%20Patches.cs#L222"); // I got annoyed so my not so passive aggressive nature isn't on display here
            Vector2 basePos = new Vector2(0, -1080);

            GameObject optionMenu = __instance.optionsMenu;
            GameObject controls = optionMenu.GetComponentInChildren<ControlsOptions>().gameObject;

            GameObject controlsUI = controls.GetComponent<ControlsOptions>().jumpText.GetComponentInParent<RectTransform>().gameObject;

            RectTransform rect = controlsUI.GetComponent<RectTransform>();
            rect.sizeDelta += new Vector2(0, 150);


            GameObject textTemplate = controlsUI.transform.GetChild(17).transform.GetChild(0).gameObject;
            GameObject keyChangeTemplate = controlsUI.transform.GetChild(2).gameObject;


            GameObject text = GameObject.Instantiate(textTemplate, controls.transform, false);
            text.GetComponent<Text>().text = "-- MOD OPTIONS --";
            text.transform.localPosition = new Vector2(0, -1080);

            Vector2 addedPos = Vector2.zero;


            foreach (var m in KeyBindings.mods)
            {
                int i = 0;
                addedPos += new Vector2(0, -200);

                //both are negative values therefore the higher value would be less
                if (addedPos.y + basePos.y < controlsUI.GetComponent<RectTransform>().sizeDelta.y)
                {
                    controlsUI.GetComponent<RectTransform>().sizeDelta += new Vector2(0, (addedPos.y * -1) + 100);
                }
                GameObject modSetContainer = new GameObject(m.Value.name + "-Setting Container");
                modSetContainer.transform.parent = controlsUI.transform;
                modSetContainer.transform.localPosition = basePos += addedPos;

                GameObject modText = GameObject.Instantiate(textTemplate.gameObject, modSetContainer.transform, false);
                modText.GetComponent<Text>().text = m.Value.name;
                modText.GetComponent<Text>().fontSize = 26;
                addedPos += new Vector2(0, -120);
                foreach (var key in KeyBindings.keys)
                {
                    if (key.Key.Contains(m.Value.name))
                    {
                        GameObject keyChangerContainer = GameObject.Instantiate(keyChangeTemplate, modSetContainer.transform, false);

                        GameObject keyChangerText = keyChangerContainer.transform.GetChild(0).gameObject;
                        keyChangerText.GetComponent<Text>().text = key.Key.Split('.')[1];

                        GameObject keyChangerBtn = keyChangerContainer.transform.GetChild(1).gameObject;
                        string[] path = key.Value.bindings[0].path.Split('/');
                        keyChangerBtn.GetComponentInChildren<Text>().text = path.Last();


                        ColorBlock colors = new ColorBlock()
                        {
                            normalColor = new Color(1, 1, 1, 1),
                            highlightedColor = new Color(0.9608f, 0.9608f, 0.9608f, 1),
                            pressedColor = new Color(0.7843f, 0.7843f, 0.7843f, 1),
                            selectedColor = new Color(0.9608f, 0.9608f, 0.9608f, 0.512f),
                            disabledColor = new Color(0.7843f, 0.7843f, 0.7843f, 0.502f),
                            colorMultiplier = 1f,
                            fadeDuration = 0.1f
                        };
                        keyChangerBtn.GetComponent<Button>().colors = colors;
                        Button.ButtonClickedEvent modsButton = keyChangerBtn.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                        modsButton.AddListener(delegate
                        {
                            KeyBindings.Bind(key.Key);
                            // keyChangerBtn.GetComponent<Button>().spriteState
                            // change btn state to selected somehow to change color to orange & stop that when binding is over
                        });

                        i++;
                    }
                }
            }



        }
    }
}
