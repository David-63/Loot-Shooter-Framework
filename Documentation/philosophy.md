# Design Philosophy

This framework is built around several core principles.

---

## Multiplayer First

Networking is considered from the beginning of development.

Gameplay systems should not be rewritten for multiplayer later.

---

## Server Authority

The server is the source of truth.

Clients are responsible for input and presentation.

The server validates gameplay.

---

## Separation of Responsibilities

Each class should have one responsibility.

Systems communicate through explicit interfaces instead of hidden dependencies.

---

## Composition over Inheritance

Favor composition whenever possible.

Avoid deep inheritance hierarchies.

---

## Package-Oriented Development

Each feature is developed as an independent package.

Packages should be reusable in other projects.

---

## Progressive Architecture

The framework starts simple.

As systems become stable, they are migrated toward Domain-Driven Design and Hexagonal Architecture.

Avoid premature abstraction.

---

## Validation Before Integration

Every new feature is first implemented in a prototype.

Only validated implementations are integrated into the framework.

---

## Performance

The framework targets multiplayer gameplay with many NPCs and projectiles.

Object pooling and allocation-free gameplay are considered core requirements.

---

## Long-Term Goal

Create a reusable multiplayer framework capable of powering multiple cooperative shooter RPG projects.