# Last Will and Testament App

This is a C# application that allows users to create, encrypt, read, edit, and delete their last will and testament. The app provides a secure way to store and manage sensitive information regarding the distribution of assets and final wishes.

## Overview

As a software engineer, the purpose of developing this app is to further my learning of the C# language and demonstrate my understanding of its syntax and features. The app showcases my ability to work with file I/O, encryption algorithms, and user input validation.

## Features

- Secure access: The app requires a 16-digit key combination to access the main menu, ensuring only authorized users can interact with the will.
- Create a will: Users can create a new will by entering a 64-character combination key for encryption. The will content is encrypted and saved to a file.
- Read the will: Users can read the encrypted will by entering the 16-digit key combination. The will is decrypted and displayed in the console.
- Edit the will: Users can edit the will by entering the 16-digit key combination and the 64-character combination key. The updated content is encrypted and saved.
- Delete the will: Users can delete the will by entering the 16-digit key combination and the 64-character combination key. The will file and encryption key file are deleted.

## Demo

Click [here](https://youtu.be/mQ6b9-GE9jE) to watch a 4-5 minute demo of the Last Will and Testament app. The video provides a walkthrough of the code and demonstrates the app's functionality.

## Development Environment

- IDE: Visual Studio Code 2022
- Programming Language: C#
- Libraries: None

## Useful Websites

- [C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Stack Overflow](https://stackoverflow.com/)
- [C# Corner](https://www.c-sharpcorner.com/)

## Future Work

- Implement additional error handling and input validation to enhance the app's robustness.
- Add support for multiple wills on a single device.
- Improve the encryption algorithm and key management for enhanced security.
- Implement a user interface for a more user-friendly experience.
