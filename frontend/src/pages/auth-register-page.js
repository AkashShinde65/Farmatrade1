import { useState } from 'react';
import { Link, useNavigate, useSearchParams } from 'react-router-dom';
import { useAuth } from '../hooks/auth-hook';
import * as authService from '../services/auth-service';

const HOME_PATH_BY_ROLE = {
  FARMER: '/farmer/dashboard',
  BUYER: '/buyer/browse-lots',
};

export function AuthRegisterPage() {
  const { registerAndLogin } = useAuth();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const initialRole = searchParams.get('role')?.toUpperCase() === 'BUYER' ? 'BUYER' : 'FARMER';

  const [role, setRole] = useState(initialRole);
  const [fullName, setFullName] = useState('');
  const [email, setEmail] = useState('');
  const [mobile, setMobile] = useState('');
  const [password, setPassword] = useState('');
  const [aadhaar, setAadhaar] = useState('');
  const [otp, setOtp] = useState('');
  const [otpSent, setOtpSent] = useState(false);
  const [otpSending, setOtpSending] = useState(false);
  const [error, setError] = useState('');
  const [submitting, setSubmitting] = useState(false);

  async function handleSendOtp() {
    setError('');
    setOtpSending(true);
    try {
      await authService.sendRegistrationOtp(email);
      setOtpSent(true);
    } catch (err) {
      setError(err.message || 'Unable to send OTP.');
    } finally {
      setOtpSending(false);
    }
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setError('');
    setSubmitting(true);
    try {
      if (!otpSent || otp.length !== 6) {
        throw new Error('Please send the OTP and enter the 6-digit code.');
      }
      await registerAndLogin(role, { fullName, email, mobile, password, aadhaar, otp });
      navigate(HOME_PATH_BY_ROLE[role]);
    } catch (err) {
      setError(err.message || 'Registration failed.');
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="auth-page">
      <div className="auth-card card">
        <div className="card-body">
          <div className="page-header">
            <h1 className="page-title">{role === 'FARMER' ? 'Register as a Farmer' : 'Register as a Buyer'}</h1>
            <p className="page-subtitle">
              {role === 'FARMER'
                ? 'List your produce and sell directly to buyers.'
                : 'Bid on or buy produce directly from farmers.'}
            </p>
          </div>

          <div className="role-tabs">
            {['FARMER', 'BUYER'].map((option) => (
              <button
                key={option}
                type="button"
                className={role === option ? 'active' : ''}
                onClick={() => setRole(option)}
              >
                {option.charAt(0) + option.slice(1).toLowerCase()}
              </button>
            ))}
          </div>

          <form className="form" onSubmit={handleSubmit}>
            <div className="field">
              <label className="field-label" htmlFor="fullName">Full name</label>
              <input id="fullName" className="input" value={fullName} onChange={(event) => setFullName(event.target.value)} required />
            </div>
            <div className="field">
              <label className="field-label" htmlFor="email">Email</label>
              <input
                id="email"
                className="input"
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                required
              />
            </div>
            <div className="field">
              <label className="field-label" htmlFor="password">Password</label>
              <input
                id="password"
                className="input"
                type="password"
                minLength={10}
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                required
              />
            </div>
            <div className="field">
              <label className="field-label" htmlFor="mobile">Phone</label>
              <input id="mobile" className="input" value={mobile} onChange={(event) => setMobile(event.target.value)} required />
            </div>
            <div className="field">
              <label className="field-label" htmlFor="aadhaar">Aadhaar number</label>
              <input
                id="aadhaar"
                className="input"
                value={aadhaar}
                onChange={(event) => setAadhaar(event.target.value)}
                maxLength={12}
                required
              />
              <span className="field-hint">12 digits.</span>
            </div>
            <div className="field">
              <label className="field-label" htmlFor="otp">Email OTP</label>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input
                  id="otp"
                  className="input"
                  inputMode="numeric"
                  pattern="[0-9]{6}"
                  maxLength={6}
                  value={otp}
                  onChange={(event) => setOtp(event.target.value.replace(/\D/g, '').slice(0, 6))}
                  required
                />
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={handleSendOtp}
                  disabled={otpSending || !email}
                >
                  {otpSending ? 'Sending…' : otpSent ? 'Resend OTP' : 'Send OTP'}
                </button>
              </div>
            </div>
            {error ? <p className="form-error">{error}</p> : null}
            <button type="submit" className="btn btn-primary" disabled={submitting}>
              {submitting ? 'Creating account…' : 'Create account'}
            </button>
          </form>

          <p>
            Already have an account? <Link to="/login">Login</Link>
          </p>
        </div>
      </div>
    </div>
  );
}
