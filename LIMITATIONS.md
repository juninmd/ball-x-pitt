# Ball-x-Pitt Limitations & Documentation

This document outlines the known limitations of the initial implementation of the "Ball-x-Pitt" core mechanic.

## 1. Local Testing Constraints
- **Unity Test Framework (Mocks)**: Due to the constraints of the sandbox environment without a full Unity editor, comprehensive mocking of Unity's native physics APIs (`Rigidbody2D`, `Collider2D`) is not currently implemented.
- **Automated Tests**: True validation of physical collisions and continuous detection edge cases requires manual smoke testing or integration testing within a functional Unity instance using the Unity Test Runner. The GitHub Action validation workflow will just verify the standard builds without executing functional/PlayMode testing.

## 2. CI/CD Local Simulation
- Attempting to simulate the game CI/CD pipeline locally via `act` tools may fail due to dependency mismatches (like missing permissions or Docker image setups tailored strictly for GitHub's native runners). Rely on the GitHub Actions UI for verifying real builds.

## 3. VFX Recycling Constraints
- The `BallPool` updates its active VFX queue in its standard `Update()` loop. While this minimizes Garbage Collection, if a large number of particles are destroyed in a single frame, the array cleanup loop may cause minor CPU overhead spikes. If performance becomes an issue in the future on low-end mobile devices, consider an event-driven recycling system over active polling.

## 4. GitHub Actions Disk Space
- The current workflow has explicit actions to remove pre-installed software on the Ubuntu-latest runners to free up disk space. If the project scales to hundreds of megabytes or gigabytes, `Free Disk Space` might need even more aggressive directory cleaning to avoid build failures.
