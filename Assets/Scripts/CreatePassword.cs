using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatePassword : MonoBehaviour
{
    [Header("Password Parameters")]
    [SerializeField] private string password;
    [SerializeField] private string characterForPassword;
    [SerializeField] private int passwordLength;
    public int PasswordLength
    {
        get { return passwordLength; }
        set { passwordLength = Mathf.Clamp(value, 1, 100); } // The password cannot be more than 100 and less than 1
    }

    [Header("Components")]
    public TMP_InputField SliderValue;
    public Slider sliderComponent;
    public TMP_InputField passwordTextComponent;
    public Toggle lowercaseComponent;
    public Toggle uppercaseComponent;
    public Toggle specialCharComponent;
    public Toggle numberComponent;

    private string lowercase = "abcdefghijklmnopqrstuvwxyz";
    private string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string number = "123456789";
    private string specialChar = " !\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";

    void Start()
    {
        SliderValue.text = PasswordLength.ToString();
        sliderComponent.value = PasswordLength;
        ChangeCharacterForPassword();
        RandomizePassword();
    }

    // When you change a toggle to remove one type of characters
    public void OnToggleChanged(bool isToggled)
    {
        ChangeCharacterForPassword();
        RandomizePassword();
    }

    // When you want to reload the password to have a new one
    public void OnClickReload()
    {
        RandomizePassword();
    }

    // When you change the length of the password using the Slider
    public void ControlPasswordLength()
    {
        SliderValue.text = sliderComponent.value.ToString();
        PasswordLength = ((int)sliderComponent.value);
        RandomizePassword();
    }

    // When you change manualy the length of the password using the InputText
    public void OnValueChanged(string text)
    {
       int tempNum;

       if (string.IsNullOrEmpty(text)) SliderValue.text = "1";
       int.TryParse(text, out tempNum);
       PasswordLength = tempNum;
       sliderComponent.value = tempNum;
       RandomizePassword();
    }

    // Generate a random password using RNGCryptoServiceProvider because that is better than the Class Random
    private void RandomizePassword()
    {
        char[] chars = characterForPassword.ToCharArray();
        byte[] data = new byte[passwordLength];

        using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
        {
            crypto.GetBytes(data);
        }
        StringBuilder result = new StringBuilder(passwordLength);
        foreach (byte b in data)
        {
            result.Append(characterForPassword[b % (characterForPassword.Length)]);
        }
        password = result.ToString();
        passwordTextComponent.text = password;
    }

    // Change the type of character that can appear in your password generator
    private void ChangeCharacterForPassword()
    {
        characterForPassword = null;
        if (lowercaseComponent.isOn) characterForPassword += lowercase;
        if (uppercaseComponent.isOn) characterForPassword += uppercase;
        if (specialCharComponent.isOn) characterForPassword += specialChar;
        if (numberComponent.isOn) characterForPassword += number;
    }

    // Save the password in your clipboard
    public void SaveInClipBoard()
    {
        GUIUtility.systemCopyBuffer = password;
    }
}
