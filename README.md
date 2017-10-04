# LogicGateSimulator

## Build status

`master`  [![Build Status](https://travis-ci.com/circlesabound/LogicGateSimulator.svg?token=jbnKArqd77syS7PRG6Fd&branch=master)](https://travis-ci.com/circlesabound/LogicGateSimulator)

`develop` [![Build Status](https://travis-ci.com/circlesabound/LogicGateSimulator.svg?token=jbnKArqd77syS7PRG6Fd&branch=develop)](https://travis-ci.com/circlesabound/LogicGateSimulator)

## Dev environment setup

- Ensure Git is configured to commit line endings Unix-style:

    On Windows: `git config --local core.autocrlf true`
    
    On Unix: `git config --local core.autocrlf input`

- You can change the external script editor for Unity in:

    `Edit -> Preferences -> External Tools -> External Script Editor`

## Project workflow

This project adopts a simplified Git Flow workflow.

Both `master` and `develop` branches are protected, meaning direct commits are disallowed and the only way these branches may be updated is by pull request. Work on user stories should be done in branches off of `develop`, and a PR back to `develop` created on completion of the user story.

Story-completing PRs into `develop` must pass build tests with Travis CI and be reviewed and approved by at least one member of each team (back-end and front-end). Note that Travis CI may take up to 20 minutes to build the project.

PRs into `master` are performed at the conclusion of each release cycle. In addition to passing build tests, the PR must be reviewed and approved by at least one member of each team.

## Pivotal Tracker integration

For commits that finish a story (i.e. PRs into `develop`), add a message with the following syntax to the end of your commit message:

`[(Finishes|Fixes|Delivers) #TRACKER_STORY_ID]`

The auto-commit bot should mark the story as finished and link the story back to the commit.

## Continuous integration with Travis CI

The Travis CI system will attempt to build the project upon push or PR creation. This process may take anywhere between 10 and 20 minutes after a build request has been processed, and upon completion will pass or fail the relevant commit. You may also receive an email from Travis CI notifying you if the build status of your branch has changed (from passing to failing or from failing to passing).

Note that the build server can only handle one build request at a time, so your build may remain in a request queue for some time. Branches and PRs that are updated with new commits will cancel any incomplete or queued builds for previous commits from the same branch or PR.
