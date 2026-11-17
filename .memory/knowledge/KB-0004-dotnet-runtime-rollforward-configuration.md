---
id: KB-0004
title: Multi-Version .NET Runtime Roll-Forward Configuration
component: build-infrastructure
domain: infrastructure
tags:
- dotnet
- rollforward
- build
- testing
status: validated
created: '2026-03-30'
updated: '2026-03-30'
depends_on: []
supported_by: []
---

## What Happened

The test runner failed to start because the target runtime version 9.0 was missing, while newer runtimes were installed.

## Key points

Add `<RollForward>LatestMajor</RollForward>` to `Directory.Build.props`.
This setting permits .NET processes to run on newer installed major runtimes automatically.

## Why It Matters

Prevents build and test failures on developer machines or CI agents that host preview or newer .NET runtimes.
Saves developer time when setting up local environments.

## Applies When

Configure build and test pipelines across heterogeneous .NET SDK installations.
