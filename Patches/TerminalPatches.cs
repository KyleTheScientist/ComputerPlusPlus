using ComputerPlusPlus.Tools;
using GorillaExtensions;
using GorillaLocomotion;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Utilla;

namespace ComputerPlusPlus.Patches
{
    [HarmonyPatch(typeof(GorillaComputerTerminal), "LateUpdate")]
    static class TerminalPatch
    {
        static List<GorillaComputerTerminal> gottem = new List<GorillaComputerTerminal>();

        private static void Postfix(GorillaComputerTerminal __instance)
        {
            if (!gottem.Contains(__instance))
            {
                gottem.Add(__instance);
                var screenText = ComputerManager.Instance.CloneAndScale(__instance.myScreenText);
                var funcText = ComputerManager.Instance.CloneAndScale(__instance.myFunctionText);
                Logging.Debug("==========================================");
                Logging.Debug("screenText: " + (screenText == null));
                Logging.Debug("funcText: " + (funcText == null));

                var mirror = __instance.gameObject.AddComponent<ComputerMirror>();
                mirror.screenText = screenText;
                mirror.functionsText = funcText;
                mirror.originalScreenText = __instance.myScreenText;
                mirror.originalFunctionsText = __instance.myFunctionText;
            }
            else
            {
                __instance.GetComponent<ComputerMirror>().UpdateText();
            }
        }
    }

    public class ComputerMirror : MonoBehaviour
    {
        public Text screenText, functionsText;
        public Text originalScreenText, originalFunctionsText;
        bool planeSet = false;
        Vector3 offset = new Vector3(-.21f, .41f, -.104f);
        Transform backgroundPlane;

        public void UpdateText()
        {
            screenText.text = ComputerManager.Instance.ScreenText;
            functionsText.text = ComputerManager.Instance.FunctionsText;
            screenText.color = ComputerManager.Instance.screenText.color;
            functionsText.color = ComputerManager.Instance.functionsText.color;
        }

        void FixedUpdate()
        {
            if (Time.frameCount % 60 == 0)
            {
                screenText.transform.localPosition = originalScreenText.transform.localPosition;
                screenText.transform.localRotation = originalScreenText.transform.localRotation;
                functionsText.transform.localPosition = originalFunctionsText.transform.localPosition;
                functionsText.transform.localRotation = originalFunctionsText.transform.localRotation;
                screenText.enabled = true;
                functionsText.enabled = true;
                originalScreenText.enabled = false;
                originalFunctionsText.enabled = false;
                backgroundPlane = Instantiate(ComputerManager.Instance.backgroundPlane);
                backgroundPlane.SetParent(screenText.transform.parent);
                SetBackgroundPosition();
            }
        }

        void SetBackgroundPosition()
        {
            RaycastHit hit;
            Ray ray = new Ray(screenText.transform.position, screenText.transform.forward);
            if (Physics.Raycast(ray, out hit, 100f, Player.Instance.locomotionEnabledLayers))
            {
                backgroundPlane.position = hit.point;
                backgroundPlane.rotation = Quaternion.LookRotation(-hit.normal);
                backgroundPlane.position += backgroundPlane.TransformDirection(-.05f, 0, -.001f);
                planeSet = true;
            }
        }

        void OnGUI()
        {
            //-.2 .41 -.103

            //make 3 sliders in the top right for each component of the offset
            //offset.x = GUI.HorizontalSlider(new Rect(0, 50, 100, 50), offset.x, -.25f, -.15f);
            //offset.y = GUI.HorizontalSlider(new Rect(0, 100, 100, 50), offset.y, .3f, .5f);
            //offset.z = GUI.HorizontalSlider(new Rect(0, 150, 100, 50), offset.z, -.105f, - .1f);
            //// add labels with the values
            //GUI.Label(new Rect(100, 50, 100, 50), offset.x.ToString());
            //GUI.Label(new Rect(100, 100, 100, 50), offset.y.ToString());
            //GUI.Label(new Rect(100, 150, 100, 50), offset.z.ToString());
        }
    }

}
