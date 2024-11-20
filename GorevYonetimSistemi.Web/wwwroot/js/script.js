document.addEventListener("DOMContentLoaded", function () {
    // Tema butonunu seçin
    const screenMode = document.getElementById('screenMode');

    // Sayfa yüklendikten sonra yerel depolamadan tema kontrolü
    const savedTheme = localStorage.getItem("theme") || "light";
    
    // Sayfa yüklendikten sonra temayı uygula
    document.documentElement.classList.add(savedTheme);
    
    // Temanın yüklenip yüklenmediğini kontrol et
    if (savedTheme === "dark") {
        screenMode.classList.add('fa-sun', 'text-white');
        screenMode.classList.remove('fa-moon', 'text-black');
    } else {
        screenMode.classList.add('fa-moon', 'text-black');
        screenMode.classList.remove('fa-sun', 'text-white');
    }

    // Tema değişim fonksiyonu
    const toggleTheme = () => {
        // Tema geçişini yap ve doğru temayı yerel depolamaya kaydet
        const isDark = document.documentElement.classList.toggle("dark");

        // Temanın yerel depolamada saklanması
        localStorage.setItem("theme", isDark ? "dark" : "light");

        // Simgeleri göster/gizle
        if (isDark) {
            screenMode.classList.add('fa-sun', 'text-white');
            screenMode.classList.remove('fa-moon', 'text-black');
        } else {
            screenMode.classList.add('fa-moon', 'text-black');
            screenMode.classList.remove('fa-sun', 'text-white');
        }
    };

    // Butona tıklama olayını ekleyin
    screenMode.addEventListener("click", () => {
        toggleTheme();
    });
});
