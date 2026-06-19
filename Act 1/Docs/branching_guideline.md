# Git Branching and Commit Guidelines

We utilize a **feature branch workflow** in this repository.  
While the way you slice your commits and branches is ultimately up to you, maintaining consistency across the team drastically improves code reviews, debugging, and collaboration.

---

## 1. Branching Strategy

### Base Branch
* **The Golden Rule:** All feature, bugfix, and chore branches must be created from `main`. 
* **Protection:** Direct pushes to `main` are strictly prohibited. All changes must be introduced via a Pull Request (PR).

### Naming Conventions
Use clear, lowercase, hyphenated names prefixed by the type of work and the project ticket ID (e.g., Jira/GitHub Issues):

* `feature/PROJ-<ticket-id>-<description>`
    * *Example:* `feature/PROJ-123-login-ui`
* `bugfix/PROJ-<ticket-id>-<description>`
    * *Example:* `bugfix/PROJ-456-cart-total`
* `hotfix/<description>` *(Used strictly for urgent, immediate production fixes)*
    * *Example:* `hotfix/security-patch`
* `docs/<description>` *(For documentation-only updates)*
    * *Example:* `docs/update-readme`

### Lifecycle & Scope
* **Atomic Scope:** Keep branches focused. **One branch = one unit of work.** If a feature grows too large, break it down into smaller, sequential sub-tasks and separate branches.
* **Repository Hygiene:** To prevent clutter, always check the **"Delete branch after merging"** option when your Pull Request is approved and merged.

---

## 2. Commit Message Standards

We follow the **Conventional Commits** lightweight specification. This helps us skim the project history quickly and can even automate our changelogs.

* **The Imperative Mood:** Always write the description in the imperative, present-tense mood (e.g., use `Add login validation`, **not** `Added login validation` or `Adds login validation`). 
* **Size:** Keep commits small and meaningful. Each commit should represent a single logical step or change.
* **Context:** Include just enough context so reviewers understand *what* changed and *why*.

---

## 3. Pull Request (PR) Workflow

1.  **Reviewers:** Assign at least one peer to review your code before merging.
2.  **Squash and Merge:** Unless specified otherwise, prefer "Squash and Merge" when closing your PR to keep the `main` history clean and linear.

---  

#### **Clarity > Bureaucracy**
> These are team conventions designed to make our lives easier, not strict handcuffs. The ultimate goal is clarity, smooth teamwork, and maintaining a healthy codebase. 