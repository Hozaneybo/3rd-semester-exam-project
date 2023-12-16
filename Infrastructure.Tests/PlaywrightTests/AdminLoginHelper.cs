using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Infrastructure.Tests.PlaywrightTests;

public static class AdminLoginHelper
{
    public static async Task AdminLoginAsync(IPage page)
    {
        await page.GotoAsync("http://localhost:5000/home");
        
        page.WaitForNavigationAsync();

        await page.Locator("ion-buttons").Filter(new() { HasText = "Login" }).ClickAsync();
        
        page.WaitForNavigationAsync();
        
        await page.GetByLabel("Username or email address").ClickAsync();

        await page.GetByLabel("Username or email address").FillAsync("test@admin.com");

        await page.GetByLabel("Password").ClickAsync();

        await page.GetByLabel("Password").FillAsync("TTTTTTTT");

        await page.GetByRole(AriaRole.Button, new() { Name = "Sign in" }).ClickAsync();
        
        page.WaitForNavigationAsync();

    }
}