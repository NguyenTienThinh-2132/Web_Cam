---
name: Heritage Lens
colors:
  surface: '#fcf9f8'
  surface-dim: '#dcd9d9'
  surface-bright: '#fcf9f8'
  surface-container-lowest: '#ffffff'
  surface-container-low: '#f6f3f2'
  surface-container: '#f0eded'
  surface-container-high: '#eae7e7'
  surface-container-highest: '#e4e2e1'
  on-surface: '#1b1c1c'
  on-surface-variant: '#4f453f'
  inverse-surface: '#303030'
  inverse-on-surface: '#f3f0f0'
  outline: '#81756e'
  outline-variant: '#d2c4bc'
  surface-tint: '#705a4c'
  primary: '#26170c'
  on-primary: '#ffffff'
  primary-container: '#3d2b1f'
  on-primary-container: '#ac9181'
  inverse-primary: '#dec1af'
  secondary: '#775a19'
  on-secondary: '#ffffff'
  secondary-container: '#fed488'
  on-secondary-container: '#785a1a'
  tertiary: '#1a1a15'
  on-tertiary: '#ffffff'
  tertiary-container: '#2f2f29'
  on-tertiary-container: '#98968f'
  error: '#ba1a1a'
  on-error: '#ffffff'
  error-container: '#ffdad6'
  on-error-container: '#93000a'
  primary-fixed: '#fbddca'
  primary-fixed-dim: '#dec1af'
  on-primary-fixed: '#28180d'
  on-primary-fixed-variant: '#574335'
  secondary-fixed: '#ffdea5'
  secondary-fixed-dim: '#e9c176'
  on-secondary-fixed: '#261900'
  on-secondary-fixed-variant: '#5d4201'
  tertiary-fixed: '#e5e2da'
  tertiary-fixed-dim: '#c9c6be'
  on-tertiary-fixed: '#1c1c17'
  on-tertiary-fixed-variant: '#474741'
  background: '#fcf9f8'
  on-background: '#1b1c1c'
  surface-variant: '#e4e2e1'
typography:
  display-lg:
    fontFamily: Playfair Display
    fontSize: 64px
    fontWeight: '700'
    lineHeight: '1.1'
    letterSpacing: -0.02em
  headline-lg:
    fontFamily: Playfair Display
    fontSize: 48px
    fontWeight: '600'
    lineHeight: '1.2'
  headline-lg-mobile:
    fontFamily: Playfair Display
    fontSize: 32px
    fontWeight: '600'
    lineHeight: '1.2'
  headline-md:
    fontFamily: Playfair Display
    fontSize: 32px
    fontWeight: '600'
    lineHeight: '1.3'
  body-lg:
    fontFamily: Work Sans
    fontSize: 18px
    fontWeight: '400'
    lineHeight: '1.6'
  body-md:
    fontFamily: Work Sans
    fontSize: 16px
    fontWeight: '400'
    lineHeight: '1.6'
  label-md:
    fontFamily: Work Sans
    fontSize: 14px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: 0.05em
rounded:
  sm: 0.125rem
  DEFAULT: 0.25rem
  md: 0.375rem
  lg: 0.5rem
  xl: 0.75rem
  full: 9999px
spacing:
  base: 8px
  section-gap: 120px
  container-max: 1280px
  gutter: 32px
  margin-mobile: 20px
  margin-desktop: 60px
---

## Brand & Style

This design system is built for a premium classic camera shop, emphasizing the craftsmanship, history, and tactile nature of analog photography. The aesthetic blends **Traditional Editorial** and **Modern Minimalism**, creating a "digital boutique" experience that feels as established as a century-old camera manufacturer.

The personality is authoritative, sophisticated, and nostalgic. It targets collectors and enthusiast photographers who value quality over convenience. The visual direction utilizes high-quality photography of mechanical details, generous whitespace, and a refined color palette to evoke a sense of timelessness and precision.

## Colors

The palette is inspired by the materials of vintage rangefinders: mahogany, brass, and textured leather.

- **Primary (Deep Wood):** A rich, dark brown used for primary text, deep backgrounds, and structural elements.
- **Secondary (Brass/Gold):** An elegant accent color for call-to-actions, highlights, and icons, mimicking the metallic finish of vintage hardware.
- **Background (Parchment/Cream):** A warm, light neutral that replaces stark white to provide a softer, more sophisticated reading experience.
- **Neutral (Charcoal):** Used for technical details, secondary text, and UI elements where high contrast against cream is required.

## Typography

The typographic hierarchy relies on the contrast between the high-contrast elegance of **Playfair Display** and the functional, modern clarity of **Work Sans**. 

Headlines should be treated with editorial care—using large sizes and tighter letter spacing for a premium feel. Body text remains highly legible on the parchment background. All labels and metadata (such as aperture settings or film speeds) use uppercase Work Sans with increased tracking to mimic the engravings found on camera lenses.

## Layout & Spacing

The layout adopts a **Fixed Grid** philosophy to maintain a structured, gallery-like feel. Large section gaps are utilized to give products room to "breathe," emphasizing their individual importance.

- **Desktop:** A 12-column grid with wide 32px gutters and significant 60px outer margins.
- **Mobile:** A single-column flow with 20px margins, where headlines scale down to ensure readability without losing their dramatic impact.
- **Rhythm:** Spacing follows a base-8 increment system. Content blocks are separated by generous vertical padding (120px) to distinguish different product categories or brand stories.

## Elevation & Depth

To maintain a sophisticated and timeless feel, this design system avoids heavy shadows or digital-first effects like glassmorphism. Instead, it uses **Tonal Layering** and **Subtle Textures**:

1.  **Planes:** Depth is created by layering Deep Wood cards or Charcoal containers over the Parchment background.
2.  **Stroke:** Elements are defined by thin (1px) brass or charcoal borders rather than shadows.
3.  **Texture:** Subtle noise or grain overlays on the cream background mimic the texture of high-quality paper or film grain.
4.  **Interactions:** Hover states should feel mechanical—buttons may slightly change in color tone or gain a refined inner stroke, suggesting a tactile "click."

## Shapes

The shape language is predominantly **Soft (0.25rem)**. This subtle rounding prevents the UI from feeling too sharp and modern while avoiding the playfulness of fully rounded corners. It mimics the slightly softened edges of a leather-bound camera body or a machined metal casing. Images of products should maintain these sharp or very slightly softened corners to preserve their technical profile.

## Components

### Buttons
- **Primary:** Solid Deep Wood background with Cream text. 1px border in Brass. Soft corners.
- **Secondary:** Ghost style. 1px Charcoal border, Charcoal text.
- **Tertiary/Text:** Uppercase Work Sans with a Brass underline that expands on hover.

### Cards
Cards use a Parchment or very light gray fill with a thin 1px border. Product images should be center-aligned with generous internal padding (at least 32px) to mimic a museum display.

### Input Fields
Inputs are minimalist: a single bottom border in Charcoal that turns Brass when focused. Labels are small, uppercase, and positioned above the field.

### Chips & Tags
Used for product status (e.g., "In Stock", "Rare"). Small, rectangular with no roundedness, using a light Brass background and Deep Wood text.

### Navigation
The top navigation should be clean and centered, using uppercase label-md typography. A small brass dot indicator should appear under the active page link.