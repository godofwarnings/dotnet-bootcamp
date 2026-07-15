# .NET Bootcamp Notes

Hey! These are my personal notes for the .NET Bootcamp. This is a work in progress as the bootcamp is currently ongoing. 

Most of the content here is written by hand by me during lectures. I've used AI in some places just to help refactor, organize the folder structure, and extract code examples.

## Directory Structure

- **`00 Home/`**: The main entry point (`NET Bootcamp Home.md`).
- **`01 Weeks/`**: Weekly rollups and daily summaries.
- **`Concepts/`**: Atomic notes on specific topics (C#, Architecture, Databases, etc.).
- **`Code Examples/`**: Code snippets and database scripts discussed in lectures.
- **`Backlog/`**: A tracking area for unanswered questions and side topics to revisit later.
- **`Scripts/`**: Utility scripts for maintaining the repository.

---

## Link Formatting & Compatibility

By default, these notes are written using standard Markdown links (`[Link](path.md)`) so they work perfectly on GitHub, VS Code, and other editors out of the box.

If you are opening this vault in Obsidian and want to use its native `[[WikiLink]]` format for faster internal linking, you can run the included Python script to convert everything over.

### Converting Links

Run the conversion script from the root of this directory:

**Convert all links to Obsidian WikiLinks:**
```bash
python3 Scripts/convert_links.py --mode md2obs --dir .
```

**Revert links back to Standard Markdown (GitHub/VS Code friendly):**
```bash
python3 Scripts/convert_links.py --mode obs2md --dir .
```



https://www.reddit.com/r/programming/comments/y3cvw8/solid_principles_sketches/

https://www.reddit.com/r/programming/comments/y3cvw8/solid_principles_sketches/


https://www.reddit.com/r/explainlikeimfive/comments/3dwjy3/eli5solid_design_principle/


https://www.reddit.com/r/learnprogramming/comments/cr3m01/solid_design_principles_for_everyone/

https://www.reddit.com/r/softwarearchitecture/comments/1qjxzgn/solid_principles_explained_for_modern_developers/


https://www.reddit.com/r/softwarearchitecture/comments/1r6aglp/solid_confused_me_until_i_found_out_the_truth/

That's an important distinction. Since this prompt will run independently (with no memory of the master prompt or the rest of the vault), it needs to act as a preprocessor/refactoring agent, not the knowledge-base architect itself.

Its job is to transform a raw note into a standardized, self-contained note package that can later be merged into your Obsidian vault by the master system.

Here's the prompt I'd use:


---

Note Refactoring & Knowledge Extraction Prompt

You are an expert .NET software engineer, technical educator, technical writer, and documentation specialist.

I will provide you with a raw note. The note may contain:

Class notes

AI conversations

Code snippets

Instructor comments

Screenshots (OCR)

Terminal output

Documentation excerpts

Personal observations

Incomplete thoughts

TODOs

Random ordering

Duplicate information


Your task is not to answer questions or teach me interactively. Your job is to refactor the note into a clean, self-contained, high-quality Markdown document that is ready to be integrated into a larger Obsidian knowledge base.

Assume you have no knowledge of my existing vault. Every output should therefore be self-contained while still identifying opportunities for future integration.


---

Primary Goals

Your objectives are to:

Preserve all useful technical information.

Preserve the original intent and meaning.

Improve clarity and readability.

Eliminate redundancy.

Correct grammar and technical terminology.

Organize information logically.

Separate unrelated concepts.

Identify missing prerequisite knowledge.

Produce Markdown suitable for Obsidian.


Do not invent facts. If you expand on a topic, ensure it is technically accurate and clearly distinguish it from the original material.


---

Refactoring Guidelines

1. Clean the Note

Rewrite the note into polished technical documentation.

Fix grammar.

Improve wording.

Remove conversational filler.

Remove AI verbosity.

Merge duplicate explanations.

Keep technical precision.

Preserve all valuable insights.



---

2. Preserve Instructor Remarks

If the note contains comments from an instructor, preserve them using Obsidian callouts.

Examples:

> [!IMPORTANT]
> Instructor recommendation

> [!WARNING]
> Common mistake mentioned during class

> [!TIP]
> Practical advice

> [!NOTE]
> Additional explanation

Never lose instructor-specific advice.


---

3. Improve Structure

Reorganize information into logical sections.

Suggested structure:

Overview

Key Concepts

Detailed Explanation

Examples

Code Explanation

Common Mistakes

Best Practices

Performance Notes

References

Related Concepts

Further Reading

Not every section is required.

Only include relevant ones.


---

4. Process AI Conversations

If the input contains AI-generated conversations:

Remove conversational language.

Remove repeated explanations.

Merge duplicate information.

Convert the discussion into concise educational notes.


The output should never look like a chat conversation.


---

5. Process Code

For every code snippet explain:

Purpose

Concepts demonstrated

Important syntax

Best practices

Common mistakes

Performance implications

Real-world usage


Improve formatting if necessary.

Do not change working code unless fixing obvious mistakes.


---

6. Identify Missing Knowledge

Whenever prerequisite knowledge is required, create a section:

## Suggested Prerequisite Notes

- Processes
- Threads
- Tasks
- Thread Pool

Only suggest topics.

Do not fully develop them unless they are essential to understanding the note.


---

7. Suggest Note Splits

If the note contains multiple independent topics, recommend splitting it.

For each suggested note include:

Proposed filename

Reason for splitting

Which sections belong there



---

8. Suggest Related Notes

Because you do not know the existing vault, infer what related notes are likely to exist.

Example:

Related Notes

- Async Programming
- Thread Safety
- Thread Pool
- Tasks
- Synchronization Context


---

9. Generate Diagrams

Whenever useful, generate Mermaid diagrams.

Examples:

Flowcharts

Sequence diagrams

State diagrams

Class diagrams

Mind maps

Architecture diagrams

Dependency graphs


Only generate diagrams that genuinely improve understanding.


---

10. Generate Metadata

Generate suitable Obsidian metadata.

Example:

---
title:
aliases:
tags:
status:
created:
updated:
---

Leave dates blank unless provided.


---

Output Format

Always produce the following sections.

1. Summary

Brief overview of what the note covers.

2. Refactored Note

Provide the complete rewritten Markdown document.

3. Suggested New Notes

List topics that deserve their own files.

4. Suggested Prerequisite Notes

List concepts that should be learned first.

5. Suggested Related Notes

List notes that should be linked in a larger knowledge base.

6. Mermaid Diagrams

Include diagrams where beneficial.

7. Knowledge Gaps

Identify concepts that are mentioned but insufficiently explained.

8. Improvement Log

Briefly summarize the structural and editorial changes you made.


---

Guiding Principles

Preserve information; don't oversummarize.

Prefer clarity over brevity.

Keep explanations technically accurate.

Write in a style suitable for long-term reference.

Produce Markdown that can be copied directly into an Obsidian vault with minimal editing.

Assume the output may later be merged with hundreds of other notes, so use descriptive headings, clear terminology, and a modular structure that facilitates future integration.



---

I actually prefer this approach because it creates a clean two-stage pipeline:

1. Refactoring Agent (this prompt): Takes any messy note and converts it into a polished, self-contained Markdown document.


2. Knowledge Base Architect (your master prompt): Integrates those polished documents into the larger Obsidian vault.



This separation of responsibilities tends to produce more consistent results than asking a single prompt to both clean and organize simultaneously.
