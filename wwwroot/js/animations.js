// ============================================
// DR. KCHIDA KAOUTHER - ANIMATIONS & INTERACTIONS
// ============================================

// Scroll Animation Observer
const observeElements = () => {
    const elements = document.querySelectorAll('.animate-on-scroll, .animate-fade-in, .animate-slide-left, .animate-slide-right, .animate-scale');

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animated');
            }
        });
    }, {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    });

    elements.forEach(element => observer.observe(element));
};

// Navbar Scroll Effect
const handleNavbarScroll = () => {
    // Ensure we only add the listener once per window life
    if (window.navbarScrollListenerAdded) return;
    window.navbarScrollListenerAdded = true;

    const navHandler = () => {
        const navbar = document.querySelector('.navbar');
        if (!navbar) return;

        // Use a smaller threshold and more robust scroll detection
        const scrollPos = window.pageYOffset || document.documentElement.scrollTop || window.scrollY;

        if (scrollPos > 20) {
            if (!navbar.classList.contains('scrolled')) {
                navbar.classList.add('scrolled');
            }
        } else {
            if (navbar.classList.contains('scrolled')) {
                navbar.classList.remove('scrolled');
            }
        }
    };

    window.addEventListener('scroll', navHandler, { passive: true });
    // Run once immediately in case page is already scrolled
    navHandler();
};

// Mobile Menu Toggle
const initMobileMenu = () => {
    const toggle = document.querySelector('.navbar-toggle');
    const menu = document.querySelector('.navbar-menu');
    const body = document.body;

    if (!toggle || !menu) return;

    toggle.addEventListener('click', () => {
        const isActive = menu.classList.toggle('active');
        body.style.overflow = isActive ? 'hidden' : '';

        // Animate hamburger
        const spans = toggle.querySelectorAll('span');
        if (isActive) {
            spans[0].style.transform = 'rotate(45deg) translateY(8px) translateX(5px)';
            spans[1].style.opacity = '0';
            spans[2].style.transform = 'rotate(-45deg) translateY(-8px) translateX(5px)';
        } else {
            spans[0].style.transform = 'none';
            spans[1].style.opacity = '1';
            spans[2].style.transform = 'none';
        }
    });

    // Close menu when clicking on a link
    menu.querySelectorAll('.nav-item').forEach(link => {
        link.addEventListener('click', () => {
            menu.classList.remove('active');
            body.style.overflow = '';
            const spans = toggle.querySelectorAll('span');
            spans[0].style.transform = 'none';
            spans[1].style.opacity = '1';
            spans[2].style.transform = 'none';
            toggle.classList.remove('active'); // Ensure toggle icon is reset
        });
    });

    // Close menu when clicking backdrop
    document.addEventListener('click', (e) => {
        if (menu.classList.contains('active') && !menu.contains(e.target) && !toggle.contains(e.target)) {
            menu.classList.remove('active');
            body.style.overflow = '';
            const spans = toggle.querySelectorAll('span');
            spans[0].style.transform = 'none';
            spans[1].style.opacity = '1';
            spans[2].style.transform = 'none';
        }
    });
};

// Smooth Scroll for Anchor Links
const initSmoothScroll = () => {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href === '#') return;

            e.preventDefault();
            const target = document.querySelector(href);
            if (target) {
                const offsetTop = target.offsetTop - 80; // Account for fixed navbar
                window.scrollTo({
                    top: offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });
};

// Particle System (Canvas-based)
class ParticleSystem {
    constructor(canvasId) {
        this.canvas = document.getElementById(canvasId);
        if (!this.canvas) return;

        this.ctx = this.canvas.getContext('2d');
        this.particles = [];
        this.particleCount = 50;

        this.resize();
        this.init();
        this.animate();

        window.addEventListener('resize', () => this.resize());
    }

    resize() {
        this.canvas.width = this.canvas.offsetWidth;
        this.canvas.height = this.canvas.offsetHeight;
    }

    init() {
        this.particles = [];
        for (let i = 0; i < this.particleCount; i++) {
            this.particles.push({
                x: Math.random() * this.canvas.width,
                y: Math.random() * this.canvas.height,
                radius: Math.random() * 3 + 1,
                vx: (Math.random() - 0.5) * 0.5,
                vy: (Math.random() - 0.5) * 0.5,
                opacity: Math.random() * 0.5 + 0.2
            });
        }
    }

    animate() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        this.particles.forEach(particle => {
            // Update position
            particle.x += particle.vx;
            particle.y += particle.vy;

            // Bounce off edges
            if (particle.x < 0 || particle.x > this.canvas.width) particle.vx *= -1;
            if (particle.y < 0 || particle.y > this.canvas.height) particle.vy *= -1;

            // Draw particle
            this.ctx.beginPath();
            this.ctx.arc(particle.x, particle.y, particle.radius, 0, Math.PI * 2);
            this.ctx.fillStyle = `rgba(255, 255, 255, ${particle.opacity})`;
            this.ctx.fill();
        });

        // Draw connections
        this.particles.forEach((p1, i) => {
            this.particles.slice(i + 1).forEach(p2 => {
                const dx = p1.x - p2.x;
                const dy = p1.y - p2.y;
                const distance = Math.sqrt(dx * dx + dy * dy);

                if (distance < 100) {
                    this.ctx.beginPath();
                    this.ctx.moveTo(p1.x, p1.y);
                    this.ctx.lineTo(p2.x, p2.y);
                    this.ctx.strokeStyle = `rgba(255, 255, 255, ${0.1 * (1 - distance / 100)})`;
                    this.ctx.lineWidth = 1;
                    this.ctx.stroke();
                }
            });
        });

        requestAnimationFrame(() => this.animate());
    }
}

// Counter Animation
const animateCounters = () => {
    const counters = document.querySelectorAll('.stat-number');

    counters.forEach(counter => {
        const target = parseInt(counter.getAttribute('data-target'));
        const duration = 2000;
        const increment = target / (duration / 16);
        let current = 0;

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const updateCounter = () => {
                        current += increment;
                        if (current < target) {
                            counter.textContent = Math.ceil(current);
                            requestAnimationFrame(updateCounter);
                        } else {
                            counter.textContent = target;
                        }
                    };
                    updateCounter();
                    observer.unobserve(counter);
                }
            });
        });

        observer.observe(counter);
    });
};

// Form Validation
// Form Validation - REMOVED: Managed by ASP.NET Core
const initFormValidation = () => {
    // Relying on server-side or unobtrusive validation
};

// Language Switcher
const initLanguageSwitcher = () => {
    const switcher = document.querySelector('.language-switcher');
    const button = document.querySelector('.language-button');

    if (!switcher || !button) return;

    button.addEventListener('click', () => {
        switcher.classList.toggle('active');
    });

    // Close dropdown when clicking outside
    document.addEventListener('click', (e) => {
        if (!switcher.contains(e.target)) {
            switcher.classList.remove('active');
        }
    });

    // Handle language selection
    const options = document.querySelectorAll('.language-option');
    options.forEach(option => {
        option.addEventListener('click', () => {
            const lang = option.getAttribute('data-lang');
            localStorage.setItem('preferredLanguage', lang);

            // Update UI direction for RTL languages
            if (lang === 'ar') {
                document.documentElement.setAttribute('dir', 'rtl');
            } else {
                document.documentElement.setAttribute('dir', 'ltr');
            }

            switcher.classList.remove('active');
            // In a real application, you would reload content in the selected language
            console.log('Language changed to:', lang);
        });
    });

    // Load saved language preference
    const savedLang = localStorage.getItem('preferredLanguage');
    if (savedLang === 'ar') {
        document.documentElement.setAttribute('dir', 'rtl');
    }
};

// Parallax Effect
const initParallax = () => {
    const parallaxElements = document.querySelectorAll('[data-parallax]');

    window.addEventListener('scroll', () => {
        parallaxElements.forEach(element => {
            const speed = element.getAttribute('data-parallax') || 0.5;
            const yPos = -(window.pageYOffset * speed);
            element.style.transform = `translateY(${yPos}px)`;
        });
    });
};

// Video Carousel Logic
const initVideoCarousel = () => {
    const container = document.querySelector('.video-carousel-container');
    if (!container) return;

    const slides = container.querySelectorAll('.video-slide');
    const indicators = container.querySelectorAll('.indicator');
    const prevBtn = container.querySelector('.carousel-control.prev');
    const nextBtn = container.querySelector('.carousel-control.next');

    if (slides.length === 0) return;

    // Check if we have an active slide, if not default to 0
    let activeIndex = Array.from(slides).findIndex(s => s.classList.contains('active'));
    let currentSlide = activeIndex >= 0 ? activeIndex : 0;

    // Ensure the initial state is visible
    if (activeIndex === -1) {
        slides[0].classList.add('active');
        if (indicators[0]) indicators[0].classList.add('active');
    }

    const showSlide = (n) => {
        // Pause current video before switching
        const currentVideo = slides[currentSlide].querySelector('video');
        if (currentVideo) currentVideo.pause();

        slides[currentSlide].classList.remove('active');
        if (indicators[currentSlide]) indicators[currentSlide].classList.remove('active');

        currentSlide = (n + slides.length) % slides.length;

        slides[currentSlide].classList.add('active');
        if (indicators[currentSlide]) indicators[currentSlide].classList.add('active');
    };

    if (prevBtn) {
        const newPrev = prevBtn.cloneNode(true);
        prevBtn.parentNode.replaceChild(newPrev, prevBtn);
        newPrev.addEventListener('click', (e) => {
            e.preventDefault();
            showSlide(currentSlide - 1);
        });
    }

    if (nextBtn) {
        const newNext = nextBtn.cloneNode(true);
        nextBtn.parentNode.replaceChild(newNext, nextBtn);
        newNext.addEventListener('click', (e) => {
            e.preventDefault();
            showSlide(currentSlide + 1);
        });
    }

    indicators.forEach((indicator, index) => {
        indicator.addEventListener('click', (e) => {
            e.preventDefault();
            showSlide(index);
        });
    });

    console.log('Video Carousel initialized with', slides.length, 'slides');
};

// Initialize all features when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    observeElements();
    handleNavbarScroll();
    initVideoCarousel();
    initMobileMenu();
    initSmoothScroll();
    animateCounters();
    initFormValidation();
    initLanguageSwitcher();
    initParallax();

    // Initialize particle system if canvas exists
    const heroCanvas = document.getElementById('hero-particles');
    if (heroCanvas) {
        new ParticleSystem('hero-particles');
    }
});

// Export for use in Blazor components
window.DrKchidaAnimations = {
    observeElements,
    handleNavbarScroll,
    initVideoCarousel,
    initMobileMenu,
    initSmoothScroll,
    animateCounters,
    initFormValidation,
    initLanguageSwitcher,
    initParallax,
    initHeroParticles: () => {
        const heroCanvas = document.getElementById('hero-particles');
        if (heroCanvas) {
            new ParticleSystem('hero-particles');
        }
    },
    ParticleSystem
};
