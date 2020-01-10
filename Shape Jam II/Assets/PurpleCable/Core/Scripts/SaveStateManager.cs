using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace PurpleCable
{
    /// <summary>
    /// Save state manager
    /// </summary>
    public class SaveStateManager
    {
        #region Data

        /// <summary>
        /// Save state data
        /// </summary>
        private static XElement SaveState { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Gets a save state's slot name
        /// </summary>
        /// <param name="slot">The slot number</param>
        /// <returns></returns>
        private static string GetSaveSlotName(int slot)
            => $"SaveState{slot}";

        /// <summary>
        /// Initialises a new save state
        /// </summary>
        public static void NewSaveState()
        {
            SaveState = new XElement("SaveState");
        }

        /// <summary>
        /// Saves a state into a slot
        /// </summary>
        /// <param name="slot">The slot number</param>
        public static void SaveSlot(int slot)
        {
            if (SaveState == null)
                NewSaveState();

            PlayerPrefs.SetString(GetSaveSlotName(slot), SaveState.ToString());
        }

        /// <summary>
        /// Loads a slot's save state
        /// </summary>
        /// <param name="slot">The slot number</param>
        /// <returns>The name of the current scene</returns>
        public static string LoadSlot(int slot)
        {
            // Get save state from PlayerPrefs
            string saveStateString = PlayerPrefs.GetString(GetSaveSlotName(slot));

            // If thaere is a save state for that slot, load it, if not, create a new one
            if (string.IsNullOrEmpty(saveStateString))
                NewSaveState();
            else
                SaveState = XElement.Parse(saveStateString);

            // Load inventory from save state
            Inventory.FromXElement(SaveState.Element("Inventory"));

            // Get current scene from save state
            return (string)SaveState.Attribute("CurrentScene");
        }

        /// <summary>
        /// Deletes a save state
        /// </summary>
        /// <param name="slot">The slot number</param>
        public static void DeleteSlot(int slot)
        {
            PlayerPrefs.DeleteKey(GetSaveSlotName(slot));
        }

        /// <summary>
        /// Saves data for the current scene
        /// </summary>
        /// <param name="sceneName">The scene name</param>
        /// <param name="elements">Formatted save data for the scene</param>
        public static void SaveScene(string sceneName, IEnumerable<XElement> elements)
        {
            // Set scene as current scene
            SaveState.Attribute("CurrentScene")?.Remove();
            SaveState.Add(new XAttribute("CurrentScene", sceneName));

            // Save scene data
            SaveState.Element(sceneName)?.Remove();
            SaveState.Add(new XElement(sceneName, elements));

            // Save inventory
            SaveState.Element("Inventory")?.Remove();
            SaveState.Add(Inventory.ToXElement("Inventory"));
        }

        /// <summary>
        /// Gets a scene's data
        /// </summary>
        /// <param name="sceneName">The name of the scene</param>
        /// <returns>The scene's data</returns>
        public static XElement LoadScene(string sceneName)
        {
            return SaveState.Element(sceneName);
        }

        #endregion
    }
}
