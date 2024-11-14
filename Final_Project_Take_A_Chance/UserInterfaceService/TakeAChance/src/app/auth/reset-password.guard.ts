import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const resetPasswordGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const email = route.queryParamMap.get('email');
  const token = route.queryParamMap.get('token');

  if (email && token) {
    return true;
  }
  router.navigate(['/']);
  return false;
};
