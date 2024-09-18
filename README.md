# Prog Poe

## Description

My **Prog Poe** is a municipality application that allows users to report issues and view pending reports. Eventually, the app will support viewing local events and news. Users can submit a report by providing a location, selecting a category, adding a description, and optionally uploading a file.

## Technologies

- **Framework**: ASP.NET (WPF)
- **Chatbot**: Ollama (AI integration)

## Installation

To set up the application:

1. **Clone the repository**: Download or clone the project from the repository.
2. **Set up the application**: Follow the usual steps for setting up a WPF desktop application.
3. **Install and configure Ollama**: 
   - Ensure you have Ollama installed on your PC. You can refer to the [Ollama Documentation for Windows](https://github.com/ollama/ollama/blob/main/docs/windows.md) for installation instructions.
   - Replace the Ollama API endpoint URL in the code with the one for your machine.
   
All other dependencies are included or not currently needed.

## Configuration Details

- To integrate **Ollama** with the application, replace the existing Ollama API endpoint URL with the one from your local Ollama setup. This allows the chatbot to function as intended.

## Usage

Currently, Prog Poe is a WPF desktop application. The user workflow is as follows:

1. Enter the **location** of the report.
2. Select a **category** for the issue (e.g., sanitation, utilities).
3. Enter a **description** of the issue.
4. Optionally, upload a file as an attachment.
5. Submit the report and view it in the **pending reports** section.

## Contributors

This project is developed and maintained by **Jarryd Harker**.
