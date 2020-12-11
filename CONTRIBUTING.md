## Contributing Guidline

- Before starting, please make sure you have read the documenations and there is no ambiguity in the implementation.

- Before submiting code for features or bug fixes in the development environment, make sure you have created a new topic branch from `k8s-plt` branch. 
    ```ps
    # Always start from k8s-plt
    git checkout k8s-plt

    # Make sure you have latest changes
    git pull origin
    git checkout -b feature/my-sample-branch

    # After making changes to the code base...

    git add .
    git commit -m "my changes for feature xxx"

    # And send changes for us...
    git push --set-upstream origin feature/my-sample-branch
    ```
    and then create a new pull request targeting `k8s-plt` branch. You can assign the pull request to yourself if you're the a maintainer member of the project.

- In emergency siturations like submiting **hotfix** changes into the production environment, create new branch from `master` and follow the same steps from the previous paragraph. Also, make sure you submit changes back to the `k8s-plt` too!

- Never commit directly into `master` or `k8s-plt` branches even if your are a maintainer or the change is as small as a simple space character! Always create a new branch and after pushing changes to the server, create a new pull request. This might seems too much but its a good practise for history/change management.

- Please do not submit changes with build error or changes that make unit tests fail. Always try to keep unit tests updated.

- If you want to have a backup from your topic branch, push it to the server with the last commit having a `WIP:` in the start of its message. This will prevent merging half-done changes by accident.

- Please always follow the [Microsoft`s Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) as much as you can.

- Please always consider testing the main scenarios and do not wait for the QA to come back with bug lists.

- Write unit tests for complex parts of the code like commission calculations, price changes and any important algorithm in your code. Unit test projects are located in the `\tests\` subfolder of the main repository.

- And let us know if you have a good suggestion to make this list even longer!