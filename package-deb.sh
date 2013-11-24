#!/usr/bin/env bash

dpkg-buildpackage -F -I*.userprefs -Iobj -I.git* -Ibin -Idebian -I*.sh -I*.png
