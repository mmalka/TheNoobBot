public class Main : WowManager.Products.IProducts
{
    internal static readonly Questing_Bot.MainForm WindowProduct = new Questing_Bot.MainForm();
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