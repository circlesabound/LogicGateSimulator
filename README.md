# LogicGateSimulator

## Dev environment setup

- Ensure Git is configured to commit line endings Unix-style:

    `git config --local core.autocrlf true`

## Pivotal Tracker integration

For commits that finish a story, add a message with the following syntax to the end of your commit message:

`[(Finishes|Fixes|Delivers) #TRACKER_STORY_ID]`

The auto-commit bot should mark the story as finished and link the story back to the commit.
