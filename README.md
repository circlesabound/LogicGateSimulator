# LogicGateSimulator

## Dev environment setup

- Ensure Git is configured to commit line endings Unix-style:

    On Windows: `git config --local core.autocrlf true`
    
    On Unix: `git config --local core.autocrlf input`

- You can change the external script editor for Unity in:

    `Edit -> Preferences -> External Tools -> External Script Editor`

## Pivotal Tracker integration

For commits that finish a story, add a message with the following syntax to the end of your commit message:

`[(Finishes|Fixes|Delivers) #TRACKER_STORY_ID]`

The auto-commit bot should mark the story as finished and link the story back to the commit.
