using static System.Net.Mime.MediaTypeNames;

namespace Dots;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
        InitializeComponent();
        MessagesReceiver();
	}

	//==================================[ RECEIVE ]==========================

	private async void MessagesReceiver()
	{
		while (true)
		{
            ReceiveWebView.Source = new Uri("http://shoshin142.wallst.ru/dots/receive.php?id=" + NickNameLabel.Text);
			await Task.Delay(1000);
        }
	}

    private void OnMessageReceived(object sender, WebNavigatedEventArgs e)
    {
		string MessageText = e.Url;
		if (MessageText.Contains("message="))
		{
			MessageText = MessageText.Substring(MessageText.IndexOf("message="));
            MessageText = MessageText.Substring(MessageText.IndexOf("|") + 1);
            MessageText = MessageText.Substring(0, MessageText.IndexOf("|"));

			string MessageToShow = "";
			while (MessageText.Contains('X'))
			{
				MessageToShow += (char)int.Parse(MessageText.Substring(0, MessageText.IndexOf('X')));
                MessageText = MessageText.Substring(MessageText.IndexOf('X') + 1);
            }

			var NewMessageLabel = new Button
			{
				Text = MessageToShow,
				FontSize = 25,
				Padding = 10,
				BackgroundColor = Color.Parse("Black"),
                TextColor = Color.Parse("White"),
                HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.End
            };

            ChatStackLayout.Add(NewMessageLabel);
			ChatScrollView.ScrollToAsync(0, ChatStackLayout.Height, true);
        }
    }

	//============================[ SEND ]==============================

    private async void OnSendMessage(object sender, EventArgs e)
    {

		string MessageToSend = "";
		int symbol;
		for (int i = 0; i < MessageEntry.Text.Length; i++)
		{
			symbol = MessageEntry.Text[i];
            MessageToSend += symbol.ToString() + 'X';
		};

		var NewMessageLabel = new Button
        {
			Text = MessageEntry.Text,
			FontSize = 25,
			Padding = 10,
			BackgroundColor = Color.Parse("Pink"),
			TextColor = Color.Parse("Black"),
			HorizontalOptions = LayoutOptions.End
		};

        ChatStackLayout.Add(NewMessageLabel);

		//Send Message
		string url = "http://shoshin142.wallst.ru/dots/send.php?";
		url += "from=" + NickNameLabel.Text;
		url += "&to=" + FriendNameEntry.Text;
		url += "&type=text";
		url += "&message=|" + MessageToSend + "|";

        SendWebView.Source = new Uri(url);

		MessageEntry.Text = "";
    }

	private async void OnCheckNickName(object sender, EventArgs e)
	{
        if (NickNameLabel.Text == "")
            NickNameLabel.Text = await DisplayPromptAsync("Nickname!", "What's your Nickname?");
    }
}

