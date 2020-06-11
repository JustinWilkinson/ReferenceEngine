# LatexReferences
A JSON based alternative to the cumbersome styling of BibTeX references.

## Code

### BibliographyGenerator
A console app that builds the generator based on the configured styles.

### Bibtex
A class library to assist with parsing of .bib files, and the translation to internal objects which we can then format as we please.

### LatexReferences
An Electron.NET GUI to assist with the creation, configuration and saving of different styles which can be used by the BibliographyGenerator.
These configured styles can then be exported to a JSON file which is read by the bibgen.exe and used to create a .bbl file used by LaTex.


### Test
An NUnit Test project containing tests for the solution.