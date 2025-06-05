Add-Migration InitialCreate -project StoryPointPlaybook.Infrastructure -startup-project StoryPointPlaybook.Api

Update-Database -project StoryPointPlaybook.Infrastructure -startup-project StoryPointPlaybook.Api