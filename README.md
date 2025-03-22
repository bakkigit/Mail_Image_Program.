# WPF Image Viewer Application 📸

This is a **WPF application** built using **C#**, **Entity Framework**, and **MS SQL Server**. The application allows users to send images via email, and the application retrieves these images to display them in a WPF window. If multiple images are received, the application loops through them in sequence.

## Features 🎯
- 📩 **Email Integration** – Retrieves images sent to a specific email.
- 🖼️ **Image Display** – Shows received images in a WPF window.
- 🔄 **Image Looping** – If multiple images are available, they rotate in a loop.
- 💾 **Database Storage** – Uses **MS SQL Server** for storing metadata.
- ⚡ **Entity Framework** – Manages database interactions seamlessly.

## Technologies Used 🛠️
- **C#** (WPF for UI)
- **Entity Framework Core**
- **MS SQL Server**
- **SMTP (Email Integration)**

## How It Works ⚙️
1. **User sends an image** via email to the configured email address.
2. **Application fetches new emails**, extracts the attached images, and saves them.
3. **Images are displayed** in the WPF window.
4. **If multiple images are available**, they transition in a loop.

## Why Use This Application? 🤔
- **Automated image retrieval** – No need for manual uploads.
- **Smooth WPF UI** – Easy navigation and image display.
- **Database-backed** – Ensures images are saved for future use.
- **Customizable** – Can extend to include additional features (e.g., filtering, tagging).

## Contribution Guidelines 🤝
Feel free to contribute by submitting issues, feature requests, or pull requests! 🚀

Happy coding! 🚀
