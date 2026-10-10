document.addEventListener("DOMContentLoaded", () => {
            /* Header Scroll Effect */
            const navbar = document.getElementById('navbar');
            window.addEventListener('scroll', () => {
                if (window.scrollY > 50) {
                    navbar.classList.add('scrolled');
                } else {
                    navbar.classList.remove('scrolled');
                }
            });

            /* Mobile Menu Toggle */
            const menuBtn = document.querySelector('.menu-mobile');
            const nav = document.querySelector('.nav');
            if (menuBtn && nav) {
                menuBtn.addEventListener('click', () => {
                    nav.classList.toggle('active');
                });
            }

            /* Scroll Reveal Observer */
            const revealElements = document.querySelectorAll('.reveal');
            const revealObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        entry.target.classList.add('active');
                    }
                });
            }, {
                threshold: 0.1,
                rootMargin: "0px 0px -40px 0px"
            });

            revealElements.forEach(el => revealObserver.observe(el));

            /* Stat Counter Animation */
            const counters = document.querySelectorAll('.stat-number');
            let hasCounted = false;

            const countUpObserver = new IntersectionObserver((entries) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting && !hasCounted) {
                        hasCounted = true;
                        counters.forEach(counter => {
                            const target = +counter.getAttribute('data-target');
                            const duration = 2000;
                            const stepTime = 20;
                            const steps = duration / stepTime;
                            const increment = target / steps;
                            let current = 0;

                            const timer = setInterval(() => {
                                current += increment;
                                if (current >= target) {
                                    counter.innerText = target + (target === 100 ? '%' : '+');
                                    clearInterval(timer);
                                } else {
                                    counter.innerText = Math.ceil(current) + (target === 100 ? '%' : '+');
                                }
                            }, stepTime);
                        });
                    }
                });
            }, { threshold: 0.5 });

            if (counters.length > 0 && counters[0].parentElement.parentElement) {
                countUpObserver.observe(counters[0].parentElement.parentElement);
            }

            /* 3D Tilt Effect */
            const tiltCards = document.querySelectorAll('[data-tilt]');
            tiltCards.forEach(card => {
                card.addEventListener('mousemove', (e) => {
                    const rect = card.getBoundingClientRect();
                    const x = e.clientX - rect.left;
                    const y = e.clientY - rect.top;
                    const centerX = rect.width / 2;
                    const centerY = rect.height / 2;
                    const rotateX = ((y - centerY) / centerY) * -6;
                    const rotateY = ((x - centerX) / centerX) * 6;
                    card.style.transform = `perspective(1000px) rotateX(${rotateX}deg) rotateY(${rotateY}deg) translateY(-8px)`;
                });

                card.addEventListener('mouseleave', () => {
                    card.style.transform = 'perspective(1000px) rotateX(0deg) rotateY(0deg) translateY(0px)';
                });
            });

            /* Button Ripple Effect */
            const rippleContainers = document.querySelectorAll('.btn-ripple');
            rippleContainers.forEach(container => {
                container.addEventListener('click', function(e) {
                    const existingRipple = this.querySelector('.ripple');
                    if (existingRipple) existingRipple.remove();

                    const circle = document.createElement('span');
                    const diameter = Math.max(this.clientWidth, this.clientHeight);
                    const radius = diameter / 2;
                    const rect = this.getBoundingClientRect();

                    circle.style.width = circle.style.height = `${diameter}px`;
                    circle.style.left = `${e.clientX - rect.left - radius}px`;
                    circle.style.top = `${e.clientY - rect.top - radius}px`;
                    circle.classList.add('ripple');

                    this.appendChild(circle);
                });
            });

            /* Custom Select Dropdowns */
            const luxurySelects = document.querySelectorAll(".luxury-select");
            luxurySelects.forEach(select => {
                const selected = select.querySelector(".luxury-selected");
                const optionItems = select.querySelectorAll(".luxury-option");
                const hiddenInput = select.parentElement.querySelector("input[type='hidden']");

                selected.addEventListener("click", (e) => {
                    e.stopPropagation();
                    luxurySelects.forEach(s => {
                        if (s !== select) s.classList.remove("open");
                    });
                    select.classList.toggle("open");
                });

                optionItems.forEach(option => {
                    option.addEventListener("click", () => {
                        const value = option.getAttribute("data-value");
                        selected.textContent = option.textContent;
                        if (hiddenInput) {
                            hiddenInput.value = value;
                        }
                        select.classList.remove("open");
                        select.classList.remove("error");
                    });
                });
            });

            document.addEventListener("click", (e) => {
                luxurySelects.forEach(select => {
                    if (!select.contains(e.target)) {
                        select.classList.remove("open");
                    }
                });
            });

            /* Toast Notification System */
            function showToast(message, type = 'success') {
                let toastContainer = document.getElementById('toastContainer');
                if (!toastContainer) {
                    toastContainer = document.createElement('div');
                    toastContainer.id = 'toastContainer';
                    toastContainer.style.cssText = 'position:fixed; bottom:30px; right:30px; z-index:9999; display:flex; flex-direction:column; gap:10px;';
                    document.body.appendChild(toastContainer);
                }

                const toast = document.createElement('div');
                toast.style.cssText = `background:${type === 'success' ? '#2b9348' : '#d62828'}; color:#fff; padding:1rem 1.5rem; border-radius:12px; box-shadow:0 10px 30px rgba(0,0,0,0.5); font-weight:600; font-size:0.95rem; display:flex; align-items:center; gap:10px; animation:fadeInUp 0.3s ease-out;`;
                toast.innerHTML = `<i class="fas ${type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle'}"></i> <span>${message}</span>`;
                
                toastContainer.appendChild(toast);

                setTimeout(() => {
                    toast.style.opacity = '0';
                    toast.style.transform = 'translateY(20px)';
                    toast.style.transition = 'all 0.3s ease';
                    setTimeout(() => toast.remove(), 300);
                }, 4000);
            }

            /* Booking Form Handling with .NET Backend sync & localStorage fallback */
            const bookingForm = document.getElementById('bookingForm');
            const bookingModal = document.getElementById('bookingModal');
            const modalMessage = document.getElementById('modalMessage');
            const closeModal = document.getElementById('closeModal');

            if (bookingForm) {
                bookingForm.addEventListener('submit', async function(e) {
                    e.preventDefault();

                    const servizioInput = document.getElementById("servizio");
                    const servizioSelect = document.getElementById("servizioSelect");

                    if (!servizioInput.value) {
                        if (servizioSelect) servizioSelect.classList.add("error");
                        showToast("Seleziona una tipologia di servizio valida.", "error");
                        return;
                    }

                    const formData = {
                        nome: document.getElementById('nome').value,
                        telefono: document.getElementById('telefono').value,
                        data: document.getElementById('data').value,
                        ora: document.getElementById('ora').value,
                        servizio: servizioInput.value,
                        ospiti: document.getElementById('ospiti').value,
                        timestamp: new Date().toISOString()
                    };

                    let backendSynced = false;

                    try {
                        const controllerSignal = new AbortController();
                        const timeoutId = setTimeout(() => controllerSignal.abort(), 4000);

                        // Chiamata POST verso l'API .NET
                        const response = await fetch('https://localhost:5001/api/prenotazioni', {
                            method: 'POST',
                            headers: {
                                'Content-Type': 'application/json'
                            },
                            body: JSON.stringify(formData),
                            signal: controllerSignal.signal
                        });

                        clearTimeout(timeoutId);

                        if (response.ok) {
                            backendSynced = true;
                            console.log('Sincronizzato con successo con il backend .NET');
                        }
                    } catch (error) {
                        console.warn('Backend .NET non raggiungibile, salvataggio di sicurezza in locale (localStorage)...', error);
                    }

                    // Salvtaggio di sicurezza in localStorage
                    try {
                        const existingBookings = JSON.parse(localStorage.getItem('pizzeria_prenotazioni') || '[]');
                        existingBookings.push(formData);
                        localStorage.setItem('pizzeria_prenotazioni', JSON.stringify(existingBookings));
                    } catch (err) {
                        console.error('Errore salvataggio locale:', err);
                    }

                    if (modalMessage) {
                        modalMessage.innerHTML = `Grazie mille <strong>${formData.nome}</strong>!<br>La tua prenotazione per <strong>"${formData.servizio}"</strong> (${formData.ospiti}) il <strong>${formData.data}</strong> alle <strong>${formData.ora}</strong> è stata registrata${backendSynced ? ' e confermata sul server .NET' : ' (salvata in locale)'}. Ti contatteremo al <strong>${formData.telefono}</strong>!`;
                    }

                    if (bookingModal) {
                        bookingModal.classList.add('active');
                    }

                    showToast("Prenotazione registrata con successo!");
                    this.reset();

                    const servizioSelected = servizioSelect ? servizioSelect.querySelector(".luxury-selected") : null;
                    const ospitiSelected = document.querySelector("#ospitiSelect .luxury-selected");
                    if (servizioSelected) servizioSelected.textContent = "Seleziona Servizio";
                    if (ospitiSelected) ospitiSelected.textContent = "2 Persone / 2 Pizze";
                    if (servizioInput) servizioInput.value = "";
                    const ospitiInput = document.getElementById('ospiti');
                    if (ospitiInput) ospitiInput.value = "2 Persone / 2 Pizze";
                });
            }

            if (closeModal && bookingModal) {
                closeModal.addEventListener('click', () => {
                    bookingModal.classList.remove('active');
                });

                bookingModal.addEventListener('click', (e) => {
                    if (e.target === bookingModal) {
                        bookingModal.classList.remove('active');
                    }
                });
            }

            /* Smooth Scroll Indicator */
            const scrollBtn = document.getElementById('scrollBtn');
            if (scrollBtn) {
                scrollBtn.addEventListener('click', () => {
                    const aboutSection = document.querySelector('#about');
                    if (aboutSection) aboutSection.scrollIntoView({ behavior: 'smooth' });
                });
            }
        });
