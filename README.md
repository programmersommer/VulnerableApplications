# VulnerableApplications
Some projects with a demonstration of vulnerabilities

[![CodeQL](https://github.com/programmersommer/VulnerableApplications/actions/workflows/codeql.yml/badge.svg)](https://github.com/programmersommer/VulnerableApplications/security/code-scanning)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=programmersommer_VulnerableApplications&metric=bugs)](https://sonarcloud.io/summary/new_code?id=programmersommer_VulnerableApplications) [![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=programmersommer_VulnerableApplications&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=programmersommer_VulnerableApplications) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=programmersommer_VulnerableApplications&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=programmersommer_VulnerableApplications) [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=programmersommer_VulnerableApplications&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=programmersommer_VulnerableApplications)

[![ShiftLeft](https://github.com/programmersommer/VulnerableApplications/actions/workflows/main.yml/badge.svg)](https://github.com/programmersommer/VulnerableApplications/actions/workflows/main.yml)

[![CodeFactor](https://www.codefactor.io/repository/github/programmersommer/vulnerableapplications/badge)](https://www.codefactor.io/repository/github/programmersommer/vulnerableapplications)

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/399f77e2eac642c5ac4a24047715c752)](https://app.codacy.com/gh/programmersommer/VulnerableApplications/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

**CookieJarOverflow** - backend side creates a cookie with HttpOnly flag, that means that cookie cannot be accessed through client side script. But after clicking on button JS script creates hundreds of "dummy" cookies and changes the content of the cookie


**CRLF** - user input is logged in this application directly. Knowing this, Intruder can play a joke and using new line symbol do a trick and write in log file that Admin controller was accessed


**MIMESniffing** - each format type should have related information. For example, it shouldn't be possible to double click on JPG file and get exe application running. This example shows that txt file could be executed as js file. To mitigate this, special header could be added to the application


**ParameterTampering** - probably the oldest and most simple example of a mistake. If you have on the form some data and some field is set as read-only then it doesn't mean that its value couldn't be changed and sent to the backend. Do not trust user input


**PathTraversal** - author of this application has been thinking that in case if file is located in a local folder, user can send file name as a parameter and get file from this folder. User wouldn't be able to access any other folder. But in fact, using OS special path symbols like "../" it is possible to get file from any folder


**PRSSI** - when browser is running in quirks mode, it is trying to fix wrong code. This could be used by a saboteur. This example when published to hosting could be called with 3 slashes at the end of URL. And this will make a small trick - CSS would be applied. To disallow for a browser quirks mode please use DOCTYPE


**Timing attack** - when custom authorization is used, it might be a situation when in case if correct login was used by the user, some operation could check password for a longer amount of time. Based on this it is possible to run brute force attack and identify correct logins


**VulnerableDesearialization** - multiple XML and JSON attacks. Just open Swagger and try them


**WorkshopApp** - application that contains multiple vulnerabilities


**ZipBomb** - this example shows how malicious zip files could fill all available space on your hosting
