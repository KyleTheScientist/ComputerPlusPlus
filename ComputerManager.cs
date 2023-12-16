using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WalkSimulator.Tools;

namespace ComputerPlusPlus
{
    public class ComputerManager : MonoBehaviour
    {
        public static ComputerManager Instance;

        // Screens
        public List<IScreen> Screens = new List<IScreen>();
        public int currentScreenIndex = 0, currentPage = 0;
        public IScreen currentScreen;

        // Background
        public Transform backgroundPlane;
        public Material backgroundMaterial;

        // Text
        public Text screenText, originalScreenText;
        public Text functionsText, originalFunctionText;
        public const string Divider = "==========================================\n";
        string screenContent, functionsContent;
        const int maxLines = 13;
        const int screenWidth = 43;
        Font font;

        private static Dictionary<string, GorillaKeyboardButton> keys
            = new Dictionary<string, GorillaKeyboardButton>();
        public static Traverse ComputerTraverse;


        void Awake() => Instance = this;

        public void RegisterScreen(IScreen screen)
        {
            Screens.Add(screen);
            Logging.Debug($"Registered Screen: {screen.Title}");
        }

        public void UnregisterScreen(IScreen screen)
        {
            Screens.Remove(screen);
        }


        public void Initialize()
        {
            currentScreen = Screens[currentScreenIndex];
            ComputerTraverse = Traverse.Create(GorillaComputer.instance);

            foreach (var button in FindObjectsOfType<GorillaKeyboardButton>())
                keys.Add(button.characterString, button);

            var bundle = AssetUtils.LoadAssetBundle("ComputerPlusPlus/cppbundle");
            font = bundle.LoadAsset<Font>("Font");
            backgroundMaterial = Instantiate(bundle.LoadAsset<Material>("m_Unlit"));
            backgroundMaterial.color = new Color(1 / 9f, 1 / 9f, 1 / 9f);
            originalScreenText = Traverse.Create(GorillaComputer.instance.screenText).Field("text").GetValue<Text>();
            screenText = CloneAndScale(originalScreenText);

            originalFunctionText = Traverse.Create(GorillaComputer.instance.functionSelectText).Field("text").GetValue<Text>();
            functionsText = CloneAndScale(originalFunctionText);
            UpdateFunctions();

            if (AssetUtils.LoadImageFromFile() is Texture2D texture)
            {
                backgroundMaterial.mainTexture = texture;
                backgroundPlane = GameObject.CreatePrimitive(PrimitiveType.Quad).transform;
                backgroundPlane.name = "C++ Background Plane";
                backgroundPlane.SetParent(screenText.transform.parent);
                backgroundPlane.localPosition = new Vector3(0, -0.286f, 0.5f);
                backgroundPlane.localScale = new Vector3(.7f, .4f, 1f);
                backgroundPlane.localRotation = screenText.transform.localRotation;
                backgroundPlane.gameObject.layer = LayerMask.NameToLayer("TransparentFX");
                backgroundPlane.GetComponent<MeshRenderer>().material = backgroundMaterial;
            }

            foreach (var screen in Screens)
            {
                try
                {
                    screen.Start();
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
            }
        }

        public Text CloneAndScale(Text original, float scaleDelta = -0.1f)
        {
            Text clone = Instantiate(original);
            clone.transform.SetParent(original.transform.parent);
            clone.transform.localPosition = original.transform.localPosition;
            clone.transform.localScale = original.transform.localScale;
            clone.transform.localRotation = original.transform.localRotation;
            clone.rectTransform.sizeDelta = new Vector2(
                clone.rectTransform.sizeDelta.x * (1 - scaleDelta),
                clone.rectTransform.sizeDelta.y * (1 - scaleDelta));
            clone.rectTransform.localScale = new Vector3(
                clone.rectTransform.localScale.x * (1 + scaleDelta),
                clone.rectTransform.localScale.y * (1 + scaleDelta),
                clone.rectTransform.localScale.z * (1 + scaleDelta));
            original.enabled = false;
            clone.font = font;
            clone.fontSize = 10;
            clone.supportRichText = true;
            clone.name = "C++ " + original.name;
            return clone;
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            if (button.characterString == "up")
            {
                // Decrement the current screen index
                currentScreenIndex--;
                // If the current screen index is less than 0, set it to the last screen
                if (currentScreenIndex < 0)
                {
                    currentScreenIndex = Screens.Count - 1;
                    currentPage = Screens.Count / maxLines;
                }
                // If the current screen index is less than the current page, decrement the current page
                if (currentScreenIndex < currentPage * maxLines)
                {
                    currentPage--;
                }
                currentScreen = Screens[currentScreenIndex];
                UpdateFunctions();
            }
            else if (button.characterString == "down")
            {
                // Increment the current screen index
                currentScreenIndex++;
                // If the current screen index is greater than the last screen, set it to the first screen
                if (currentScreenIndex >= Screens.Count)
                {
                    currentScreenIndex = 0;
                    currentPage = 0;
                }
                // If the current screen index is greater than the current page, increment the current page
                if (currentScreenIndex >= (currentPage + 1) * maxLines)
                {
                    currentPage++;
                }
                currentScreen = Screens[currentScreenIndex];
                UpdateFunctions();
            }
            currentScreen.OnKeyPressed(button);
        }

        void UpdateFunctions()
        {
            FunctionsText = "";
            int maxLength = 9;

            for (int i = 0; i < maxLines; i++)
            {
                if (Screens.Count <= i + currentPage * maxLines)
                    break;
                var screen = Screens[i + currentPage * maxLines];
                string title = screen.Title.ToUpper().Trim();
                if (title.Length > maxLength)
                    title = title.Substring(0, maxLength);
                if (screen == currentScreen)
                    FunctionsText += ">";
                else
                    FunctionsText += " ";
                FunctionsText += title + "\n";
            }
            if (Screens.Count > (currentPage + 1) * maxLines)
                FunctionsText += " ...";
        }

        string Template =
            "{0}\n" +
            Divider +
            "{1}\n" +
            Divider +
            "\n" +
            "{2}\n";

        void FixedUpdate()
        {
            if (currentScreen == null)
                return;

            if (originalScreenText)
                originalScreenText.enabled = false;
            if (originalFunctionText)
                originalFunctionText.enabled = false;
            var text = "";
            if (currentScreen.Title.Length > 0) {
                text += Center(currentScreen.Title.ToUpper()) + "\n";
                text += Divider;
            }
            if (currentScreen.Description.Length > 0)
            {
                text += currentScreen.Description.ToUpper() + "\n";
                text += Divider;
            }
            text += currentScreen.GetContent().ToUpper();
            ScreenText = text;
        }



        void OnDisable()
        {
            screenText.enabled = false;
            functionsText.enabled = false;
            originalFunctionText.enabled = true;
            originalScreenText.enabled = true;
        }

        public static string Center(string text, char padWith = ' ')
        {
            int width = Divider.Length;
            int padding = (width - text.Length) / 2;
            string result = "";
            for (int i = 0; i < padding; i++)
                result += padWith;
            result += text;
            for (int i = 0; i < padding; i++)
                result += padWith;
            return result;
        }

        public static string Scrolling(string text, int speed = 5, int width = screenWidth)
        {
            if (text.Length < width)
                return text;
            text += " --- ";
            int start = (int)(Time.time * speed) % text.Length;
            return
                text.Substring(start, Mathf.Min(width, text.Length - start))
                + text.Substring(0, Mathf.Max(0, width - (text.Length - start)));
        }

        public string ScreenText
        {
            get
            {
                return screenContent;
            }
            set
            {
                screenContent = value;
                if (screenText)
                    screenText.text = screenContent;
            }
        }

        public string FunctionsText
        {
            get
            {
                return functionsContent;
            }
            set
            {
                functionsContent = value;
                if (functionsText)
                    functionsText.text = functionsContent;
            }
        }

        public string EnabledColor =>
            "#" + ColorUtility.ToHtmlStringRGB(screenText.color.Lighter(.3f));

        public string DisabledColor =>
            "#" + ColorUtility.ToHtmlStringRGB(screenText.color.Darker(.3f));

        public static Dictionary<string, GorillaKeyboardButton> Keys
        {
            get { return keys; }
        }

        public static GorillaKeyboardButton GetKey(string key)
        {
            return keys[key];
        }

        public static GorillaKeyboardButton GetKey(int key)
        {
            return keys[key.ToString()];
        }

        public static Traverse Field(string fieldName)
        {
            return ComputerTraverse.Field(fieldName);
        }
    }
}
