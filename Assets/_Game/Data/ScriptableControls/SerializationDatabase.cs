using UnityEngine;

namespace Asteria.Data
{
    /// <summary>
    /// Serialization configuration database for the game.
    /// Contains all serialization parameters.
    /// </summary>
    [CreateAssetMenu(menuName = "Asteria/Serialization Database")]
    public sealed class SerializationDatabase : ScriptableObject
    {
        [Header("Format")]
        public string serializationFormat = "json";
        public bool prettyPrint = true;
        public bool includeNullValues = false;

        [Header("Compression")]
        public bool enableCompression = false;
        public string compressionAlgorithm = "gzip";
        public int compressionLevel = 6;

        [Header("Encryption")]
        public bool enableEncryption = false;
        public string encryptionAlgorithm = "aes-256";

        [Header("Validation")]
        public bool enableValidation = true;
        public bool strictMode = false;
    }
}
