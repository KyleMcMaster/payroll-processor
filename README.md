# Payroll Processor

Sample HRIS application where a list of employees and their payroll information would be available in report format.

## Build status

### Api

![dotnet core payroll-processor-api - build](https://github.com/KyleMcMaster/payroll-processor/workflows/dotnet%20core%20payroll-processor-api%20-%20build/badge.svg)

### Client

![.github/workflows/npm.yml](https://github.com/KyleMcMaster/payroll-processor/workflows/.github/workflows/npm.yml/badge.svg) [![Styled with Prettier](https://img.shields.io/badge/code_style-prettier-ff69b4.svg)](https://prettier.io)

### Functions

![dotnet core payroll-processor-functions - build](https://github.com/KyleMcMaster/payroll-processor/workflows/dotnet%20core%20payroll-processor-functions%20-%20build/badge.svg?branch=master)

## Motivation

This project was created to explore a variety of technologies and patterns I use daily in one place to get a better understanding of those technologies.

Things I would like to explore:

- Functional Programming
- Automated Tests
- Basics of Angular, TypeScript, Rxjs, and Bootstrap
- Synergy that exists between Azure Functions and .Net Core Web Apis
  - Implementations of comparable endpoints in both projects to evaluate the tooling and ecosystems
- Having fun!

## Roadmap

- MVP

  - Initial Employee CRUD functionality
  - Initial Payroll CRUD functionality
  - Basic charting and analytics of payroll data

- TODOs

  - Move seed data to CosmosDB document instead of boilerplate c# code
  - Refactor client to use Akita state management
  - Toggle between Api and Azure Function backends
  - Advanced analytics like payroll totals by department, risk, and time

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://github.com/KyleMcMaster"><img src="https://avatars1.githubusercontent.com/u/11415127?v=4" width="100px;" alt=""/><br /><sub><b>Kyle McMaster</b></sub></a><br /><a href="#design-KyleMcMaster" title="Design">🎨</a> <a href="https://github.com/KyleMcMaster/payroll-processor/commits?author=KyleMcMaster" title="Code">💻</a> <a href="https://github.com/KyleMcMaster/payroll-processor/commits?author=KyleMcMaster" title="Tests">⚠️</a></td>
    <td align="center"><a href="https://www.seangwright.me"><img src="https://avatars3.githubusercontent.com/u/1382768?v=4" width="100px;" alt=""/><br /><sub><b>Sean G. Wright</b></sub></a><br /><a href="#design-seangwright" title="Design">🎨</a> <a href="https://github.com/KyleMcMaster/payroll-processor/commits?author=seangwright" title="Code">💻</a> <a href="https://github.com/KyleMcMaster/payroll-processor/pulls?q=is%3Apr+reviewed-by%3Aseangwright" title="Reviewed Pull Requests">👀</a></td>
  </tr>
</table>

<!-- markdownlint-enable -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!

## References

[Akita-Demo](https://github.com/seangwright/akita-demo)

## Build / Run

This project uses VS Code [Multi-root Workspaces](https://code.visualstudio.com/docs/editor/multi-root-workspaces).
For the best developer experience, open the workspace directly with VS Code (`code payroll-processor.code-workspace`)
or open the root of the repository in VS Code (`code .`) and when prompted, open the workspace.

### .NET

The Azure Functions solution (`Payroll.Processor.Functions.sln`) is set up as the default solution
for [Omnisharp](https://github.com/OmniSharp/omnisharp-vscode), and is loaded as soon as the
VS Code workspace is opened.

If you want to switch to work on the API solution, use the "Omnisharp: Select Project" command from the command palette
and select `Payroll.Processor.Api.sln`.

### Client

- Run the "Client: Serve" VS Code task (this will install packages and start serving the app)

- Optional: Run any of the following tasks

  - Client: Build

### Functions

- Ensure the [Azure Functions](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)
  VS Code extension is installed.

- Run "Azure Functions: Install or Update Azure Functions Core Tools" from the command palette.

- [Update Powershell security policy](https://github.com/Azure/azure-functions-core-tools/issues/1821#issuecomment-586925919)

- F5 or run from the VS Code Debug drop down "Function: Run & Attach (Debug)"

- Optional: Run any of the following tasks

  - Function: Run (Debug)
  - Function: Test
  - Function: Build (Debug)
