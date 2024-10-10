using System;
using System.ComponentModel.DataAnnotations;

namespace TamsApi.InputModels
{
    [Serializable]
    public class RefreshChallenge
    {
        public RefreshChallenge() { } /* Default ctor needed for deserialization */

        public RefreshChallenge(string refreshToken)
        {
            RefreshToken = refreshToken;
        }

        [Required]
        public string RefreshToken { get; set; }
    }
}
