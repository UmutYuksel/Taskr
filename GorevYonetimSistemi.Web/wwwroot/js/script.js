document.addEventListener("DOMContentLoaded", function () {
    // Tema butonlarını seçin
    const lightModeButton = document.getElementById("lightMode");
    const darkModeButton = document.getElementById("darkMode");

    // Sayfa yüklendikten sonra yerel depolamadan tema kontrolü
    const savedTheme = localStorage.getItem("theme") || "light";
    

    // Sayfa yüklendikten sonra temayı uygula
    document.documentElement.classList.add(savedTheme);
    
    // Temanın yüklenip yüklenmediğini kontrol et
    if (savedTheme === "dark") {
        lightModeButton.classList.add("hidden");
        darkModeButton.classList.remove("hidden");
    } else {
        lightModeButton.classList.remove("hidden");
        darkModeButton.classList.add("hidden");
    }

    // Tema değişim fonksiyonu
    const toggleTheme = () => {
        const isDark = document.documentElement.classList.toggle("dark");

        // Temanın yerel depolamada saklanması
        localStorage.setItem("theme", isDark ? "dark" : "light");

        // Simgeleri göster/gizle
        if (isDark) {
            lightModeButton.classList.add("hidden");
            darkModeButton.classList.remove("hidden");
        } else {
            lightModeButton.classList.remove("hidden");
            darkModeButton.classList.add("hidden");
        }
    };

    // Butonlara tıklama olaylarını ekleyin
    lightModeButton.addEventListener("click", () => {
        toggleTheme();
    });
    darkModeButton.addEventListener("click", () => {
        toggleTheme();
    });
});
