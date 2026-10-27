# GitHub Copilot Instructions for Slave Rebellions Improved (Continued) Mod

## Mod Overview and Purpose
The "Slave Rebellions Improved (Continued)" mod is an update for the original "busted_bunnys" mod, offering a new and improved experience for slave rebellions in the RimWorld game. This mod enhances the realism and predictability of slave rebellions, making the game's interactions more dynamic and engaging for players.

## Key Features and Systems
### New Feature: Suppression Threshold
- Slaves will not rebel if their suppression is above 60% (adjustable in mod settings).

### Rebellion Type Generation
- Rebellion types are now generated based on the number of rebelling slaves, potentially leading to grand rebellions that even suppressed slaves (up to 80% of suppression) may join.

### Rebellion vs. Escape
- The decision of whether a rebellion will be a full-scale rebellion or just an escape is determined by the availability of guns to the rebels. If slaves don't have weapons, they will simply escape.

### New AI Behavior
- Escaping slaves now exhibit new AI behavior, focusing on escaping rather than fighting or destroying property. They will prioritize running for their lives.

### Improved Alerts
- Unsuppressed slaves alert now shows the names of all unsuppressed slaves.
- Rebellion is likely alert now points to all slaves that will likely cause a rebellion, allowing cruel slave owners to prevent rebellions by beating slaves before they start.

## Coding Patterns and Conventions
### General Guidelines
- Follow RimWorld's coding standards and conventions.
- Use meaningful variable, method, and class names.
- Write clean and maintainable code.
- Include comments to explain complex logic.

### File Structure
- Organize code into logical files and directories. For example:
  - `DefOfClass.cs` for definition-related classes.
  - `LordToil_SlavesEscapeNoViolence.cs` for specific lord toil implementations.
  - `SlaveSettings.cs` for handling mod settings.
  - `StartUp.cs` for initialization code.
  - `SlaveMod.cs` for main mod logic.

### Naming Conventions
- Use PascalCase for classes, methods, and properties.
- Use camelCase for local variables and parameters.

### XML Integration
- Ensure XML files are properly formatted and integrated with the mod.
- Use XML comments to describe the purpose and usage of XML elements.

### Harmony Patching
- Use Harmony patches to modify vanilla RimWorld behavior.
- Follow best practices for patching to avoid conflicts and ensure stability.

## Suggestions for Copilot
### C# Code Suggestions
- Request code snippets for new classes and methods, especially those related to AI behavior, rebellion generation, and mod settings.
- Ask for assistance with complex logic, such as determining rebellion types based on suppression levels.
- Request code for handling XML data and integrating it with the mod.

### XML Integration
- Ask for help with XML structure and element definitions.
- Request examples of how to integrate XML data with C# code.

### Harmony Patching
- Request guidance on creating and applying Harmony patches.
- Ask for examples of how to modify vanilla RimWorld behavior using Harmony.

### General Development
- Request suggestions for improving mod performance and stability.
- Ask for tips on handling user settings and preferences.

By following these guidelines and utilizing GitHub Copilot, developers can create high-quality and engaging mod enhancements for RimWorld.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

