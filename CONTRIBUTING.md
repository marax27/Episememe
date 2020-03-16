# Contributing guidelines

## Basic workflow
1. Pick a task/bug from the project board's *To Do* list. Assign the task to yourself.
2. Create a new local Git branch named `task/task-name` or `bug/bug-name`. Usually you'll want `master` as a base branch.
```
git checkout -b task/task-name
```
3. Implement a functionality. Keep in mind:
    - **Commits**
        - Short, understandable description: *"add validation in XYZ"*, *"refactor ABC class"*, *"fix: Search button in responsive mode"* etc. If you feel the commit description is too long (more than 60~70 characters), consider splitting your changes into multiple commits.
    - **Tests**: we want more of these. An untestable code is often a bad code.
    - **Static analysis**: Visual Studio and Resharper or C#; Vue and TypeScript-related VS Code plugins for the frontend.
4. Once a feature is implemented, ensure that all changes have been pushed to GitHub.
5. Open a Pull Request and assign another team member to a **Reviewer** role.
6. Resolve Code Review comments should any appear.
7. Once the Pull Request is approved, merge your changes with the base branch and close the Pull Request.
