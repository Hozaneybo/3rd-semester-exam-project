using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class HomePage : PageTest
{
    
    [Test]
    public async Task HomepageHasLearningPlatformInTitle()
    {
        await Page.GotoAsync("http://localhost:5000/home");
        
        await Expect(Page).ToHaveTitleAsync(new Regex("Learning Platform"));
        
    }

    [Test]
    public async Task RestrictionForReviewingCourse()
    {
        await Page.GotoAsync("http://localhost:5000/home");
        await Page.Locator("div.courses-container").ClickAsync();
        
        var restrictionForLoginWindow =  Page.GetByText("To view the full content of the courses");
        Expect(restrictionForLoginWindow);
    }

    [Test]
    public async Task ClickOnSendEmailFromFooter()
    {
        await Page.GotoAsync("http://localhost:5000/home");
        
        var emailLink =  Page.GetByRole(AriaRole.Link, new() { Name = "flexserve.mailer@gmail.com" });
        
        string href = await emailLink.GetAttributeAsync("href");
        
        Assert.IsTrue(href.StartsWith("mailto:"), "The link does not lead to an email address.");
        
        await emailLink.ClickAsync();
    }

    [Test]
    public async Task Login()
    {
        await Page.GotoAsync("http://localhost:5000/home");
        
        var navigationTask = Page.WaitForNavigationAsync();
        
        var loginButton = Page.GetByRole(AriaRole.Link, new() { Name = "Login" });
        await Expect(loginButton).ToBeEnabledAsync();
        await loginButton.ClickAsync();
        
        await navigationTask;
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/login"));
    }
    
}