ReSharper Support for the N-Triples Language
-----------------------------------------------------------------



The project is a comprehensive support for a dialect of the N-Triples language. N-Triples is a line-based, plain text format for encoding an [RDF](http://en.wikipedia.org/wiki/Resource_Description_Framework) graph. See [here](http://www.w3.org/TR/rdf-testcases/#ntriples) and [here](http://www.w3.org/DesignIssues/Notation3) for details.

## Feature List

Currently the features below are completely implemented.

1. General
      - Syntax highlighting
      - Syntax error recovery
2. Code Completion
      - Namespace completion
      - LocalName completion
      - Uri completion
      - Keywords completion (context dependent)
3. Code Inspection (Highlightings)
    3. Errors
      - Syntax error highlighting
      - Unresolved prefix error highlighting
      - URI validation highlighting
      - Prefix URI validation highlighting
      - URI identifier validation highlighting
    3. Warnings
      - Duplicated prefix highlighting
      - Not-defined URI highlighting
      - Suspicious property declaration highlighting
    3. Hints
      - Statement simplification highlighting
      - Fact simplification highlighting
      - Matching brace highlighting
4. Intentions
      - Create prefix from usage intention
      - Simplify statement intention
      - Simplify fact intention
5. Refactoring
    5. Rename
      - Prefix rename refactoring
      - Local name rename refactoring
6. Navigation
      - Find usages
      - Goto on-click navigation
      - Goto file member navigation
      - Goto symbol navigation
7. Typing assist
      - Matching brace typing assist

## TODO list

1. General
     - Usage declaration path highlighting (syntax highlighting feature)
     - Solution-wide setting for predefined values (now are hard-coded)
     - Solution-wide cache optimization
     - Optimize using highlighting
     - Prefix usage mouse-over tooltip
2. Code Completion
     - Reduced completion set for automatic completion
     - Property-only completion set for type declaration smart completion
     - Custom icons for URI identifiers declaring types
3. Code Inspections
     - Separated highlighting (to be able to suppress a particular highlighting)
     - Predefined URI identifiers white-list (configurable in solution-wide settings)
     - Gutter marks left of type declaration identifiers
     - Even more code inspections (detailed use-case analysis to be done)
4. Intentions
     - Fix URI intention
6. Navigation
     - Goto type navigation
     - Goto symbol navigation rework
     - File-structure view
7. Typing assistance
     - Smart enter typing assist (including tab-indentation)
     - Erase typing assist
     - Smart brace matching assist
8. Code clean-up
     - Implementation
     - Typing-assistance integration
9. Code-manipulation
     - Statements move
     - Primitive statement members move

