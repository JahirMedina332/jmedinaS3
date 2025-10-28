

namespace jmedinaS3.View;

public partial class vLogin : ContentPage
{
	private string  [ ] users = { "Carlos", "Ana", "Jose" };
	private string [ ] pass = { "carlos123", "ana123", "jose123" };
    public vLogin()
	{
		InitializeComponent();
	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = userEntry.Text;
        string password = passwordEntry.Text;

        if (ValidateCredentials(username, password))
        {
            await DisplayAlert("�xito", $"Bienvenido {username}", "OK");
            await Navigation.PushAsync(new View.vBienvenida());
        }
        else
        {
            await DisplayAlert("Error", "Usuario o contrase�a incorrectos", "OK");
        }
    }

    private bool ValidateCredentials(string username, string password)
    {
        for (int i = 0; i < users.Length; i++)
        {
            if (users[i] == username && pass[i] == password)
            {
                return true;
            }
        }
        return false;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
	{
		// Navegar a la p�gina de registro
		await Navigation.PushAsync(new View.vRegister());
	}
}