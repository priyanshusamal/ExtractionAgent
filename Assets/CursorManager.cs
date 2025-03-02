
using UnityEngine;

namespace ExtractionAgent
{
    public class CursorManager : MonoBehaviour
    {
        [Header("Use Texture of 128x128 Resolution")]
        public Texture2D cursorTexture;
        
        public void Start()
        {
            Cursor.SetCursor(cursorTexture,new Vector2(64f,64f),CursorMode.ForceSoftware);   
        }
        
    }
}
