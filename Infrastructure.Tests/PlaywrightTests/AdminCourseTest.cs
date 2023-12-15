using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Infrastructure.Tests.PlaywrightTests;

[TestFixture]
public class AdminCourseTest : PageTest
{
    
    [SetUp]
    public void Setup()
    {
        Helper.CreateAndDeleteAdminUser(create: true, delete: false);
    }

    [TearDown]
    public void Teardown()
    {
        Helper.CreateAndDeleteAdminUser(create: false, delete: true);
    }
    
    
    [Test]
    public async Task CreateCourseTest()
    {
        
        await AdminLoginHelper.AdminLoginAsync(Page);
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Courses" }).ClickAsync();

        var showCoursesList =  Page.Locator("div").Filter(new() { HasText = "All Courses" });
        await Expect(showCoursesList).ToBeVisibleAsync();

        await Page.Locator("div").Filter(new() { HasText = "All Courses" }).GetByRole(AriaRole.Button).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/create-course"));

        await Page.GetByLabel("Title").ClickAsync();

        await Page.GetByLabel("Title").FillAsync("Create Course Test");

        await Page.GetByLabel("Description").ClickAsync();

        await Page.GetByLabel("Description").FillAsync("Here it is only test to create a course ");

        await Page.GetByLabel("Course Image URL").ClickAsync();

        await Page.GetByLabel("Course Image URL").FillAsync("course-photo");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Course" }).ClickAsync();

        var createCourseMessage = Page.GetByText("Successfully created");
        await Expect(createCourseMessage).ToBeVisibleAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/courses"));

        await Page.Locator("div").Filter(new() { HasText = "Create Course TestHere it is" }).Nth(1).ClickAsync();

        var courseBeCreated =  Page.GetByRole(AriaRole.Heading, new() { Name = "Create Course Test" });
        await Expect(courseBeCreated).ToBeVisibleAsync();
        
    }

    [Test]
    public async Task UpdateCourse()
    {
        
        await AdminLoginHelper.AdminLoginAsync(Page);
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Courses" }).ClickAsync();
        
        await Page.Locator("div:nth-child(5) > .course-info > .course-actions > .update-btn > .button-native").ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/course/update"));
        
        await Page.GetByLabel("Title").ClickAsync();

        //await Page.GetByLabel("Title").FillAsync("Update Course");

        await Page.FillAsync("#title", "Course be updated");
        
        await Page.GetByRole(AriaRole.Button, new() { Name = "Update Course" }).ClickAsync();
        
        var updateCourseMessage = Page.GetByText("Successfully updated");
        await Expect(updateCourseMessage).ToBeVisibleAsync();
        
        Helper.DeleteCourseByTitle("Course be updated");
        
    }

    /*[Test]
    public async Task DeleteCourse()
    {
        //Helper.CreateAndDeleteAdminUser(true, false);
        
        await AdminLoginHelper.AdminLoginAsync(Page);
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:5000/admin/dashboard"));
        
        await Page.GetByRole(AriaRole.Link, new() { Name = "Courses" }).ClickAsync();
        
        var deleteButtonLocator = Page.Locator($"text={"Course be updated"}").Locator("xpath=..").Locator("xpath=..").Locator(".delete-btn");
        await deleteButtonLocator.ClickAsync();

        var confirmationDialog = Page.Locator("ion-alert");
        await confirmationDialog.WaitForAsync();
        await confirmationDialog.Locator("button:has-text('Confirm')").ClickAsync();

        var deleteCourse = Page.GetByText("Successfully deleted");
        await Expect(deleteCourse).ToBeVisibleAsync();
        
        //Helper.CreateAndDeleteAdminUser(false, true);
        
    }*/
}