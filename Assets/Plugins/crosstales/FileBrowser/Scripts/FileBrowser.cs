using UnityEngine;

namespace Crosstales.FB
{
    /// <summary>Native file browser various actions like open file, open folder and save file.</summary>
    //[ExecuteInEditMode]
    //[DisallowMultipleComponent]
    //[HelpURL("https://www.crosstales.com/media/data/assets/rtvoice/api/class_crosstales_1_1_r_t_voice_1_1_speaker.html")] //TODO set correct URL
    public class FileBrowser : MonoBehaviour
    {
        private static Wrapper.IFileBrowser platformWrapper;

        /*
#region Variables

[Header("Custom Wrapper")]
/// <summary>Custom wrapper for FileBrowser.</summary>
[Tooltip("Custom wrapper for FileBrowser.")]
public Wrapper.FileBrowserBase CustomWrapper;

/// <summary>Enables or disables the custom wrapper (default: false).</summary>
[Tooltip("Enable or disable the custom wrapper (default: false).")]
public bool CustomMode = false;


[Header("Behaviour Settings")]
/// <summary>Don't destroy gameobject during scene switches (default: true).</summary>
[Tooltip("Don't destroy gameobject during scene switches (default: true).")]
public bool DontDestroy = true;

private static FileBrowser instance;
private static GameObject go;
private static bool loggedOnlyOneInstance = false;

#endregion
*/

        /*
        #region Events

        private static SingleFileSelected _onSingleFileSelected;

        /// <summary>An event triggered whenever a single file is selected.</summary>
        public static event SingleFileSelected OnSingleFileSelected
        {
            add { _onSingleFileSelected += value; }
            remove { _onSingleFileSelected -= value; }
        }
        
        #endregion


        #region Static properties

        /// <summary>Enables or disables MaryTTS.</summary>
        public static Wrapper.FileBrowserBase CustomFileBrowserWrapper
        {
            get
            {
                if (instance != null)
                {
                    return instance.CustomWrapper;
                }

                return null;
            }

            set
            {
                if (instance != null && instance.CustomWrapper != value)
                {
                    instance.CustomWrapper = value;

                    //ReloadProvider();
                }
            }
        }

        /// <summary>Enables or disables the custom voice provider.</summary>
        public static bool isCustomMode
        {
            get
            {
                if (instance != null)
                {
                    return instance.CustomMode;
                }

                return false;
            }

            set
            {
                if (instance != null && instance.CustomMode != value)
                {
                    instance.CustomMode = value;

                    //ReloadProvider();
                }
            }
        }
        
        #endregion

        #region MonoBehaviour methods

        public void Update()
        {
            if (Util.Helper.isEditorMode)
            {
                if (go != null)
                {
                    if (Util.Config.ENSURE_NAME)
                        go.name = Util.Constants.FB_SCENE_OBJECT_NAME; //ensure name
                }
            }
        }

        public void OnEnable()
        {
            //if (Util.Helper.isEditorMode || !initalized)
            if (instance == null)
            {
                if (Util.Constants.DEV_DEBUG)
                    Debug.Log("Creating new 'FileBrowser' instance");

                //                if (speaker == null)
                //                {
                instance = this;

                go = gameObject;

                go.name = Util.Constants.FB_SCENE_OBJECT_NAME;

                // Subscribe event listeners
                Wrapper.FileBrowserBase.OnSingleFileSelected += onSingleFileSelected;

                initWrapper();

                if (!Util.Helper.isEditorMode && DontDestroy)
                {
                    DontDestroyOnLoad(transform.root.gameObject);
                }
            }
            else
            {
                if (Util.Constants.DEV_DEBUG)
                    Debug.Log("Re-using 'FileBrowser' instance");

                if (!Util.Helper.isEditorMode && DontDestroy && instance != this)
                {
                    if (!loggedOnlyOneInstance)
                    {
                        Debug.LogWarning("Only one active instance of 'FileBrowser' allowed in all scenes!" + System.Environment.NewLine + "This object will now be destroyed.");

                        loggedOnlyOneInstance = true;
                    }

                    Destroy(gameObject, 0.2f);
                }
            }
        }

        public void OnDisable()
        {
            if (instance == this)
            {
                Wrapper.FileBrowserBase.OnSingleFileSelected -= onSingleFileSelected;

                //unsubscribeCustomEvents();
            }
        }

        #endregion

                    private static void onSingleFileSelected(string file)
        {
            if (_onSingleFileSelected != null)
            {
                _onSingleFileSelected(file);
            }

            Debug.Log("onSingleFileSelected: " + file);
        }

        */

        #region Constructor


        static FileBrowser()
        {
            /*
            if (CustomWrapper != null && CustomMode)
            {
                platformWrapper = CustomFileBrowserWrapper;
            }
            else 
            */
            if (Util.Helper.isEditor)
            {
                //Debug.Log("FileBrowserEditor");
#if UNITY_EDITOR
                platformWrapper = new Wrapper.FileBrowserEditor();
#endif
            }
            else if (Util.Helper.isMacOSPlatform)
            {
                //Debug.Log("FileBrowserMac");
#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
                platformWrapper = new Wrapper.FileBrowserMac();
#endif
            }
            else if (Util.Helper.isWindowsPlatform)
            {
                //Debug.Log("FileBrowserWindows");
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
                platformWrapper = new Wrapper.FileBrowserWindows();
#endif
            }
            else if (Util.Helper.isLinuxPlatform)
            {
                //Debug.Log("FileBrowserLinux");
#if UNITY_STANDALONE_LINUX && !UNITY_EDITOR
                platformWrapper = new Wrapper.FileBrowserLinux();
#endif
            }
            else
            {
                //Debug.Log("FileBrowserGeneric");
                platformWrapper = new Wrapper.FileBrowserGeneric();
            }

            //Debug.Log(platformWrapper);
        }

        #endregion


        #region Public methods

        /// <summary>Open native file browser for a single file.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extension">Allowed extension, e.g. "png"</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static string OpenSingleFile(string title, string directory, string extension)
        {
            return OpenSingleFile(title, directory, getFilter(extension));
        }

        /// <summary>Open native file browser for a single file.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <returns>Returns a string of the chosen file. Empty string when cancelled</returns>
        public static string OpenSingleFile(string title, string directory, ExtensionFilter[] extensions)
        {
            return platformWrapper.OpenSingleFile(title, directory, extensions);
        }

        /// <summary>Open native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extension">Allowed extension, e.g. "png"</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static string[] OpenFiles(string title, string directory, string extension, bool multiselect)
        {
            return OpenFiles(title, directory, getFilter(extension), multiselect);
        }

        /// <summary>Open native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static string[] OpenFiles(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
        {
            return platformWrapper.OpenFiles(title, directory, extensions, multiselect);
        }

        /// <summary>Open native folder browser for a single folder.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory (default: current, optional)</param>
        /// <returns>Returns a string of the chosen folder. Empty string when cancelled</returns>
        public static string OpenSingleFolder(string title, string directory = "")
        {
            return platformWrapper.OpenSingleFolder(title, directory);
        }

        /// <summary>
        /// Open native folder browser for multiple folders.
        /// NOTE: Multiple folder selection isnt't supported on Windows!
        /// </summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory (default: current, optional)</param>
        /// <param name="multiselect">Allow multiple folder selection (default: true, optional)</param>
        /// <returns>Returns array of chosen folders. Zero length array when cancelled</returns>
        public static string[] OpenFolders(string title, string directory = "", bool multiselect = true)
        {
            return platformWrapper.OpenFolders(title, directory, multiselect);
        }

        /// <summary>Open native save file browser</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extension">File extension, e.g. "png"</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static string SaveFile(string title, string directory, string defaultName, string extension)
        {
            return SaveFile(title, directory, defaultName, getFilter(extension));
        }

        /// <summary>Open native save file browser</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static string SaveFile(string title, string directory, string defaultName, ExtensionFilter[] extensions)
        {
            return platformWrapper.SaveFile(title, directory, defaultName, extensions);
        }

        /// <summary>Open native file browser for multiple files.</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extension">Allowed extension, e.g. "png"</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <param name="cb">Callback for the async operation.</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static void OpenFilesAsync(string title, string directory, string extension, bool multiselect, System.Action<string[]> cb)
        {
            OpenFilesAsync(title, directory, getFilter(extension), multiselect, cb);
        }

        /// <summary>Open native file browser for multiple files (async).</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="multiselect">Allow multiple file selection</param>
        /// <param name="cb">Callback for the async operation.</param>
        /// <returns>Returns array of chosen files. Zero length array when cancelled</returns>
        public static void OpenFilesAsync(string title, string directory, ExtensionFilter[] extensions, bool multiselect, System.Action<string[]> cb)
        {
            //System.Threading.Thread worker = new System.Threading.Thread(() => platformWrapper.OpenFilesAsync(title, directory, extensions, multiselect, cb));
            //worker.Start();
            platformWrapper.OpenFilesAsync(title, directory, extensions, multiselect, cb);
        }

        /// <summary>Open native folder browser for multiple folders (async).</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="multiselect"></param>
        /// <param name="cb">Callback for the async operation.</param>
        /// <returns>Returns array of chosen folders. Zero length array when cancelled</returns>
        public static void OpenFoldersAsync(string title, string directory, bool multiselect, System.Action<string[]> cb)
        {
            //System.Threading.Thread worker = new System.Threading.Thread(() => platformWrapper.OpenFoldersAsync(title, directory, multiselect, cb));
            //worker.Start();
            platformWrapper.OpenFoldersAsync(title, directory, multiselect, cb);
        }

        /// <summary>Open native save file browser</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extension">File extension, e.g. "png"</param>
        /// <param name="cb">Callback for the async operation.</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static void SaveFileAsync(string title, string directory, string defaultName, string extension, System.Action<string> cb)
        {
            SaveFileAsync(title, directory, defaultName, getFilter(extension), cb);
        }

        /// <summary>Open native save file browser (async).</summary>
        /// <param name="title">Dialog title</param>
        /// <param name="directory">Root directory</param>
        /// <param name="defaultName">Default file name</param>
        /// <param name="extensions">List of extension filters. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="cb">Callback for the async operation.</param>
        /// <returns>Returns chosen file. Empty string when cancelled</returns>
        public static void SaveFileAsync(string title, string directory, string defaultName, ExtensionFilter[] extensions, System.Action<string> cb)
        {
            //System.Threading.Thread worker = new System.Threading.Thread(() => platformWrapper.SaveFileAsync(title, directory, defaultName, extensions, cb));
            //worker.Start();
            platformWrapper.SaveFileAsync(title, directory, defaultName, extensions, cb);
        }

        /// <summary>
        /// Find files inside a path.
        /// </summary>
        /// <param name="path">Path to find the files</param>
        /// <param name="extension">Extension for the file search</param>
        /// <param name="isRecursive">Recursive search (default: false, optional)</param>
        /// <returns>Returns array of the found files inside the path. Zero length array when an error occured.</returns>
        public static string[] GetFiles(string path, string extension, bool isRecursive = false)
        {
            return GetFiles(path, getFilter(extension), isRecursive);
        }

        /// <summary>
        /// Find files inside a path.
        /// </summary>
        /// <param name="path">Path to find the files</param>
        /// <param name="extensions">List of extension filters for the find. Filter Example: new ExtensionFilter("Image Files", "jpg", "png")</param>
        /// <param name="isRecursive">Recursive search (default: false, optional)</param>
        /// <returns>Returns array of the found files inside the path. Zero length array when an error occured.</returns>
        public static string[] GetFiles(string path, ExtensionFilter[] extensions, bool isRecursive = false)
        {
            try
            {
                if (extensions == null)
                {
                    return System.IO.Directory.GetFiles(path, "*", isRecursive ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);
                }
                else
                {
                    System.Collections.Generic.List<string> files = new System.Collections.Generic.List<string>();

                    foreach (ExtensionFilter extensionFilter in extensions)
                    {
                        foreach (string ext in extensionFilter.Extensions)
                        {
                            files.AddRange(System.IO.Directory.GetFiles(path, "*." + ext, isRecursive ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly));
                        }
                    }

                    return files.ToArray();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Could not scan the path for files: " + ex);
            }

            return new string[0];
        }

        /// <summary>
        /// Find directories inside a path without recursion.
        /// </summary>
        /// <param name="path">Path to find the directories</param>
        /// <param name="isRecursive">Recursive search (default: false, optional)</param>
        /// <returns>Returns array of the found directories inside the path. Zero length array when an error occured.</returns>
        public static string[] GetDirectories(string path, bool isRecursive = false)
        {
            try
            {
                return System.IO.Directory.GetDirectories(path, "*", isRecursive ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Could not scan the path for directories: " + ex);
            }

            return new string[0];
        }

        #endregion


        #region Private methods

        private static ExtensionFilter[] getFilter(string extension)
        {
            return string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter(string.Empty, extension) };
        }

        #endregion
    }

    /// <summary>Filter for extensions.</summary>
    public struct ExtensionFilter
    {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string filterName, params string[] filterExtensions)
        {
            Name = filterName;
            Extensions = filterExtensions;
        }
    }
}
// © 2017-2019 crosstales LLC (https://www.crosstales.com)