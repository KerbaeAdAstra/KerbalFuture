# Contributing to Kerbal Future

First of all, thank you for considering contributing to Kerbal Future. We're a small team - only six people at the moment - so we can and will appreciate all the help we can get.

Following these guidelines tells us that you respect our time. In turn, we'll reciprocate that respect by addressing your issue in a timely and friendly fashion.

Kerbal Future is an open source project and we welcome contributions from the open source community. There are many ways to contribute; tutorials, documentation, bug reports - and, of course, writing code to be integrated into Kerbal Future itself.

Please don't use the issue tracker for support, questions, or other miscellany. The official IRC channel (#kerbaeadastra on Freenode) can help you with your issue.

## Ground Rules

There is only one important ground rule, and it is as follows:
> Be welcoming to newcomers and encourage diverse new contributors from all backgrounds. See our [Code of Conduct](http://35.190.136.99:3000/KerbaeAdAstra/KerbalFuture/blob/Documentation/CODE_OF_CONDUCT.md).

## Contributing

* Each merge request should implement one, and only one, feature or bug fix. Should you wish to implement or fix more than one feature/bug, submit more than one merge request.
* The commits of each merge request should be directly relevant to the merge request itself. Do not commit changes to files that are irrelevant to the merge request.
* Do not add `using` directives that point to third-party libraries, unless said library is either essential to the project or pre-approved. When in doubt, contact the project maintainers.
* Pre-approved libraries are defined as any library included with Kerbal Space Program itself (i.e., in `<KSP_DIR>/KSP_Data`).
* Be willing to accept constructive criticism from the project maintainers.
* Be aware that the merge request review process is not immediate, and that the project maintainers have other things to do. Do not pester the project maintainers.
* Low-effort merge requests will generally be rejected.

### Coding style

Please refer to the [style guide](http://35.190.136.99:8080/KerbaeAdAstra/KerbalFuture/blob/Documentation/STYLEGUIDE.md) for specific information. If non-compliant code is written, we may ask you to refactor it accordingly.

### Commit message guidelines

The commit message should describe what changed and why.

1. The first line should:
   * contain a short description of the change
   * be 50 characters or less
   * be entirely in lowercase with the exception of proper nouns, acronyms, and programming terms
   * be prefixed with the name of the changed subsystem and start with an imperative verb.

   Examples:
   * `doc: fix typos in code of conduct`
   * `cfg: add missing PartModule to Spacefolder`

1. Keep the second line blank.
1. Wrap all other lines at 72 columns.

1. If your patch fixes an open issue, you can add a reference to it at the end of the log. Use the `Fixes:` prefix and the full issue URL. For other references, use `Refs:`.

   Examples:
   * `Fixes: http://35.190.136.99:8080/KerbaeAdAstra/KerbalFuture/issues/76`
   * `Refs: http://35.190.136.99:8080/KerbaeAdAstra/KerbalFuture/merge_requests/83`

Sample complete commit message:

```plaintext
subsystem: explain the commit in one line

Body of commit message is a few lines of text, explaining things
in more detail, possibly giving some background about the issue
being fixed, etc.

The body of the commit message can be several paragraphs, and
please do proper word-wrap and keep columns shorter than about
72 characters or so. That way, `git log` will show things
nicely even when it is indented.

Fixes: http://35.190.136.99:8080/KerbaeAdAstra/KerbalFuture/issues/76
Refs: http://35.190.136.99:8080/KerbaeAdAstra/KerbalFuture/merge_requests/83
```

## Bug Reporting

### Security Vulnerabilities

If you find a security vulnerability, **DO NOT** open an issue! Send an email to kerbaeadastra@gmail.com at your earliest convenience.

### Bug Reporting Etiquette

To make our work easier, please answer these questions when submitting a bug report:

1. What version of Kerbal Space Program is the mod running on?
1. What version is the mod itself?
1. What operating system and processor architecture are you using?
1. What did you do that caused this bug?
1. What exceptions were thrown?

And most importantly, you **must** include the `KSP.log` (preferably using the GitLab Snippet function, or thru Pastebin) when submitting a bug report. **NO LOGS == NO SUPPORT.**

## Suggesting Features

Kerbal Future is based on a foundation of hard science-fiction. Handwaving is kept to a minimum. Kerbal Future will implement most if not all paradigms of Kerbae ad Astra - see the official Wiki [here](http://35.190.136.99/w/). As such, feature requests or suggestions will probably be denied unless it fits in with the universe.

## Community

You can reach the team in several ways. The preferred way is of course IRC - the channel is #kerbaeadastra on Freenode. Alternatively, send an email to kerbaeadastra@gmail.com entitled "To [TEAMMEMBER]".
