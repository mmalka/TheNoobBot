public class Main : WowManager.Products.IProducts
{
    public static Battleground_Bot.MainForm WindowProduct = new Battleground_Bot.MainForm();

    public void Initialize()
    {
        WindowProduct.Show();
    }

    public void Dispose()
    {
        try
        {
            WindowProduct.Hide();
            WindowProduct.Dispose();
        }
        catch { }
    }
}

