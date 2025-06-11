Add-Migration InitialCreate -project StoryPointPlaybook.Infrastructure -startup-project StoryPointPlaybook.API

Update-Database -project StoryPointPlaybook.Infrastructure -startup-project StoryPointPlaybook.API
