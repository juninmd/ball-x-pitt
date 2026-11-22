# AGENTS.md

Ball-x-Pitt: a Unity 6 game (Universal Render Pipeline). The code lives under the `NeonDefense` namespace; the project and some docs still use the older `Ball-X-Pitt` and `NeonDefense` names.

## Stack

- Unity Editor `6000.3.6f1` (`ProjectSettings/ProjectVersion.txt`).
- C#, `NeonDefense` assembly (`Assets/Scripts/NeonDefense/NeonDefense.asmdef`).
- Universal Render Pipeline, Input System (`InputSystem_Actions.inputactions`).
- Packages are declared in `Packages/manifest.json`.

## Commands

- Open the project with Unity Editor `6000.3.6f1`. The repo has no CLI build or test script.
- Setup for a new machine: `SETUP.md`.
- Release: push a tag `v*`. `deploy.yml` runs on tags only and uses the `UNITY_LICENSE` secret.
- Validation criteria: `VERIFICATION_REPORT.md`. Every major change must meet them.

## Structure

- `Assets/Scripts/`: game code. `Assets/Scenes/`: scenes. `Assets/Settings/`: project settings. `Assets/Tests/`: tests.
- `Assets/Scripts/NeonDefense.asmdef`: assembly definition. Keep new code inside this assembly.
- `Packages/`: package manifest and lock file. `ProjectSettings/`: Unity project settings.
- `.github/workflows/`: `validate.yml` (PR and main), `security.yml`, `sast.yml`, `deploy.yml` (tags), `release-drafter.yml`.

## Conventions

- Keep each script under 150 lines. Split the logic into smaller components when it grows.
- Zero mocks. Use real physics and real game data for tests wherever possible.
- Strict typing. Avoid `object` and untyped containers. Use typed interfaces and classes.
- Plan, Act, Validate for every new game mechanic.
- Use `SETUP.md` to onboard new developers.

## Gotchas

- Do not change `m_EditorVersion` in `ProjectVersion.txt` as a side effect. The editor version is pinned on purpose.
- Scene and asset `.meta` files carry GUIDs. Never delete a `.meta` without its asset, and never copy assets without their `.meta`.
- `GEMINI.md` is an agent audit log. The Gemini CLI may load it as context, so check before you remove or move it.

<!-- base-actions-main-policy -->
## GitHub Actions — padrão obrigatório (@main)

- Referencie as actions internas sempre pelo branch, nunca por SHA:
  - `uses: juninmd/base-actions/<action>@main`
  - `uses: juninmd/base-actions/.github/workflows/<arquivo>.yml@main`
- Não fixe `@<sha>` em `juninmd/base-actions`: a atualização é centralizada no
  `main` e passa a valer no próximo run.
- PRs cujos checks não iniciam por billing/spending limit são mesclados pelo bot central
  `juninmd/base-actions/.github/workflows/auto-merge-billing-blocked.yml`.
  Para impedir o merge automático de um PR, aplique a label `do-not-merge`.
