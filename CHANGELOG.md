# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.1+2.02.21] - 2025-03-04

### Fixed

- Fixed event instances in audio events wanting to be started when they're set to null.

## [1.1.0+2.02.21] - 2025-03-04

### Added

- Added the option to provide instance created callbacks when playing an audio event.
- Specific callback methods to set volume, pitch, and parameters.

### Changed

- Removed PlayInstance methods from audio effect in favor of callbacks.

## [1.0.2+2.02.21] - 2025-02-19

### Added

- Added methods to the audio event class to play and return its instance, stop, stop all and manage global instances.

## [1.0.1+2.02.21] - 2024-12-27

### Changed

- Updated source code license from GPL 3.0 to LGPL 3.0, so that larger works can use the library without disclosing their source code.

## [1.0.0+2.02.21] - 2024-09-29

### Added

- State vectors for playing audio events in 3D in multiple ways.

### Changed

- Added paths of FMOD studio components in the property drawers in the editor to search for them easier.

## [0.2.0+2.02.21] - 2024-06-16

### Changed

- Updated code for the property drawers in the editor.

## [0.1.1+2.02.19] - 2024-02-22

### Changed

- Updated the readme file.

## [0.1.0+2.02.19] - 2024-01-14

### Added

- C# bindings for FMOD Studio and FMOD Unity classes.
- Search windows for FMOD Studio components, i.e. banks, buses and VCAs.
