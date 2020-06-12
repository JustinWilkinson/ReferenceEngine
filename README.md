# LatexReferences
A JSON based alternative to the cumbersome styling of BibTeX references.
This is still in active development and does not yet fully support 

## Code

### BibliographyGenerator
A console app that builds the generator based on the configured styles.

### Bibtex
A class library to assist with parsing of .bib files, and the translation to internal objects which we can then format as we please.

### LatexReferences
An Electron.NET GUI to assist with the creation, configuration and saving of different styles which can be used by the BibliographyGenerator.
These configured styles can then be exported to a JSON file which is read by the bibgen.exe and used to create a .bbl file used by LaTeX.

### Test
An NUnit Test project containing tests for the solution.

## Usage
The bibgen.exe can simply be used in place of Bibtex. It reads in a lib.bib file and formats it based on the contents of the style.json file provided in configuration.